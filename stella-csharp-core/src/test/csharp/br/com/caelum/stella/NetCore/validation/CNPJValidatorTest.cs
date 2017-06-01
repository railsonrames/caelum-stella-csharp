using Caelum.Stella.CSharp.Error;
using Caelum.Stella.CSharp.Validation.Error;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Caelum.Stella.CSharp.Validation.Test
{
    [TestClass]
    public class CNPJValidatorTest : BaseDocumentValidatorTest
    {
        private CNPJValidator cnpjValidator;
        private String validString = "26.637.142/0001-58";

        [TestInitialize()]
        public void Initialize()
        {
            cnpjValidator = new CNPJValidator();
        }

        [TestMethod]
        public void ShouldValidateValidCNPJ()
        {
            cnpjValidator.IsValid("11222333000181");
            cnpjValidator.IsValid("63025530002409");
            cnpjValidator.IsValid("61519128000150");
            cnpjValidator.IsValid("68745386000102");
        }

        [TestMethod]
        public void ShoulValidateNullCNPJ()
        {
            cnpjValidator.IsValid(null);
        }

        [TestMethod]
        public void ShouldNotValidateCNPJCheckDigitsWithFirstCheckDigitWrong()
        {
            CNPJValidator validator = new CNPJValidator();
            // VALID CNPJ = 742213250001-30
            try
            {
                String value = "74221325000160";
                cnpjValidator.IsValid(value);
                Assert.Fail();
            }
            catch (InvalidStateException e)
            {
                Assert.IsTrue(e.GetErrors().Count == 1);
                AssertMessage(e, DocumentError.InvalidCheckDigits);
            }
        }

        [TestMethod]
        public void ShouldNotValidateCNPJCheckDigitsWithSecondCheckDigitWrong()
        {
            CNPJValidator validator = new CNPJValidator();

            // VALID CNPJ = 266371420001-58
            try
            {
                String value = "26637142000154";
                cnpjValidator.IsValid(value);
                Assert.Fail();
            }
            catch (InvalidStateException e)
            {
                Assert.IsTrue(e.GetErrors().Count == 1);
                AssertMessage(e, DocumentError.InvalidCheckDigits);
            }
        }

        [TestMethod]
        public void ShouldValidateValidFormattedCNPJ()
        {
            CNPJValidator validator = new CNPJValidator(true);
            String value = validString;
            validator.IsValid(value);
        }

        [TestMethod]
        public void ShouldNotValidateValidUnformattedCNPJWhenExplicity()
        {
            CNPJValidator validator = new CNPJValidator(true);

            // VALID CNPJ = 26.637.142/0001-58
            try
            {
                String value = "26637142000158";
                validator.IsValid(value);
                Assert.Fail();
            }
            catch (InvalidStateException e)
            {
                Assert.IsTrue(e.GetErrors().Count == 1);
                AssertMessage(e, DocumentError.InvalidFormat);
            }
        }
    }
}
