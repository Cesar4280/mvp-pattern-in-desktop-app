using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CRUDWinFormsMVP.Presenters.Common
{
    public class ModelDataValidation
    {
        public void Validate(object model)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(model);
            bool isValid = Validator.TryValidateObject(model, context, results, true);
            if (!isValid)
            {
                var errorMessage = new StringBuilder();
                foreach (var item in results)
                {
                    errorMessage.Append("- ");
                    errorMessage.AppendLine(item.ErrorMessage);
                }
                throw new Exception(errorMessage.ToString());
            }
        }
    }
}
