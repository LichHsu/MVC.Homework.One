using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace MVC.Homework.One.Validates
{
    public class PhoneValidateAttribute : DataTypeAttribute
    {
        string validFormat = @"\d{4}-\d{6}";
        public PhoneValidateAttribute() :base (DataType.Text)
        {
            this.ErrorMessage = "電話格式不符.";
        }

        public override bool IsValid(object value)
        {
            if (value == null) { return false; }

            return Regex.IsMatch(value.ToString(), validFormat);
        }

    }
}