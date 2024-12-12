using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MF_FD_SA_AUTH_MANAGER.Filters
{
    #region Common validation

    /// <summary>
    /// Date format: DD-MMM-YYYY
    /// </summary>
    public class DATE : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            try
            {
                if (!string.IsNullOrEmpty(Convert.ToString(value)))
                {
                    DateTime parseDate = DateTime.ParseExact(value.ToString(), "dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                }
            }
            catch (Exception)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }

    /// <summary>
    /// Date format: DD-MMM-YYYY
    /// </summary>
    public class NoPastDate : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            try
            {
                if (!string.IsNullOrEmpty(Convert.ToString(value)))
                {
                    DateTime parseDate = DateTime.ParseExact(value.ToString(), "dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    if (parseDate < DateTime.Now)
                    {
                        return new ValidationResult("Date can not be past date");

                    }
                }
            }
            catch (Exception)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }

    /// <summary>
    /// Date format: DD-MMM-YYYY
    /// </summary>
    public class NoFutureDate : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            try
            {
                if (!string.IsNullOrEmpty(Convert.ToString(value)))
                {
                    DateTime parseDate = DateTime.ParseExact(value.ToString(), "dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    if (parseDate > DateTime.Now)
                    {
                        return new ValidationResult("Date can not be future date");

                    }
                }
            }
            catch (Exception)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }

    public class PAN : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            if (!string.IsNullOrEmpty(Convert.ToString(value)))
            {
                bool match = Regex.IsMatch(value.ToString(), "^[a-zA-Z]{5}[0-9]{4}[a-zA-Z]{1}$");

                if (!match)
                    return new ValidationResult("PAN Number is not valid");
            }
            return ValidationResult.Success;
        }
    }

    public class EMAIL : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            if (!string.IsNullOrEmpty(Convert.ToString(value)))
            {
                bool match = Regex.IsMatch(value.ToString(),
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$");

                if (!match)
                    return new ValidationResult(ErrorMessage);
            }
            return ValidationResult.Success;
        }
    }

    public class MOBILE : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(value)))
            {
                bool match = Regex.IsMatch(value.ToString(), @"^[0-9]*$");

                if (!match)
                    return new ValidationResult(ErrorMessage);

                if (value.ToString().Length != 10)
                    return new ValidationResult(ErrorMessage);

            }
            return ValidationResult.Success;
        }
    }

    public class NumberOnly : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(value)))
            {
                bool match = Regex.IsMatch(value.ToString(), @"^[0-9]*$");

                if (!match)
                    return new ValidationResult(ErrorMessage);
            }
            return ValidationResult.Success;
        }
    }

    public class CharacterOnly : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(value)))
            {
                bool match = Regex.IsMatch(value.ToString(), @"^[0-9]*$");

                if (!match)
                    return new ValidationResult(ErrorMessage);
            }
            return ValidationResult.Success;
        }
    }

    public class RequiredIf : ValidationAttribute
    {
        public string PropertyName { get; set; }
        public object Value { get; set; }

        public RequiredIf(string propertyName, object value, string errorMessage = "")
        {
            PropertyName = propertyName;
            ErrorMessage = errorMessage;
            Value = value;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var instance = validationContext.ObjectInstance;
            var type = instance.GetType();
            var proprtyvalue = type.GetProperty(PropertyName).GetValue(instance, null);
            if (proprtyvalue.ToString() == Value.ToString() && value == null)
            {
                return new ValidationResult(ErrorMessage);
            }
            return ValidationResult.Success;
        }
    }

    public class RequiredIfNotNull : ValidationAttribute
    {
        public string PropertyName { get; set; }

        public RequiredIfNotNull(string propertyName, string errorMessage = "")
        {
            PropertyName = propertyName;
            ErrorMessage = errorMessage;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var instance = validationContext.ObjectInstance;
            var type = instance.GetType();
            var proprtyvalue = type.GetProperty(PropertyName).GetValue(instance, null);
            if (!string.IsNullOrEmpty(proprtyvalue.ToString()) && value == null)
                return new ValidationResult(ErrorMessage);

            return ValidationResult.Success;
        }
    }
    #endregion


    #region GenDoc API validation

    //public class DocTypeInvaid : ValidationAttribute
    //{
    //    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    //    {
    //        if (value != null && Convert.ToString(value) != "")
    //        {
    //            if (!ErrorMsg.DocTypes.Contains(Convert.ToString(value)))
    //            {
    //                return new ValidationResult(ErrorMessage);
    //            }
    //        }
    //        return ValidationResult.Success;
    //    }
    //}

    //public class DocSubTypeInvaid : ValidationAttribute
    //{
    //    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    //    {
    //        if (value != null && Convert.ToString(value) != "")
    //        {
    //            if (!ErrorMsg.DocSubTypes.Contains(Convert.ToString(value)))
    //            {
    //                return new ValidationResult(ErrorMessage);
    //            }
    //        }
    //        return ValidationResult.Success;
    //    }
    //}


    public class ApplNoRequired : ValidationAttribute
    {

        public string _DocSubType { get; set; }

        public ApplNoRequired(string DocSubType, string errorMessage = "")
        {
            _DocSubType = DocSubType;
            ErrorMessage = errorMessage;
        }

        //protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        //{
        //    var instance = validationContext.ObjectInstance;
        //    var type = instance.GetType();
        //    var proprtyvalue = type.GetProperty(_DocSubType).GetValue(instance, null);

        //    if (proprtyvalue != null && Convert.ToString(proprtyvalue) != "" && value == null)
        //    {
        //        if (ErrorMsg.DocSubTypeForApplNo.Contains(Convert.ToString(proprtyvalue)))
        //        {
        //            return new ValidationResult(ErrorMessage);
        //        }
        //    }
        //    return ValidationResult.Success;
        //}
    }


    public class FolioNoRequied : ValidationAttribute
    {
        public string _DocSubType { get; set; }

        public FolioNoRequied(string DocSubType, string errorMessage = "")
        {
            _DocSubType = DocSubType;
            ErrorMessage = errorMessage;
        }

        //protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        //{
        //    var instance = validationContext.ObjectInstance;
        //    var type = instance.GetType();
        //    var proprtyvalue = type.GetProperty(_DocSubType).GetValue(instance, null);
        //    if (proprtyvalue != null && value == null)
        //    {
        //        if (ErrorMsg.DocSubTypeForFolio.Contains(proprtyvalue))
        //        {
        //            return new ValidationResult(ErrorMessage);
        //        }
        //    }
        //    return ValidationResult.Success;
        //}
    }

    /// <summary>
    /// Required if DocSubType='EFDR' and ApplNo is empty
    /// </summary>
    //public class FdrNoRequied : ValidationAttribute
    //{
    //    public string _DocSubType { get; set; }
    //    public string _ApplNo { get; set; }

    //    public FdrNoRequied(string DocSubType, string ApplNo, string errorMessage = "")
    //    {
    //        _DocSubType = DocSubType;
    //        _ApplNo = ApplNo;
    //        ErrorMessage = errorMessage;
    //    }

    //    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    //    {
    //        var instance = validationContext.ObjectInstance;
    //        var type = instance.GetType();
    //        var DocSubType = type.GetProperty(_DocSubType).GetValue(instance, null);
    //        var ApplNo = type.GetProperty(_ApplNo).GetValue(instance, null);

    //        if (ApplNo == null)
    //        {
    //            if (DocSubType != null && value == null)
    //            {
    //                if (ErrorMsg.DocSubTypeFoFDR.Contains(DocSubType))
    //                {
    //                    return new ValidationResult(ErrorMessage);
    //                }
    //            }
    //        }
    //        return ValidationResult.Success;
    //    }
    //}

    public class FdrNoRequied : ValidationAttribute
    {
        public string _DocSubType { get; set; }

        public FdrNoRequied(string DocSubType, string errorMessage = "")
        {
            _DocSubType = DocSubType;
            ErrorMessage = errorMessage;
        }

        //protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        //{
        //    var instance = validationContext.ObjectInstance;
        //    var type = instance.GetType();
        //    var DocSubType = type.GetProperty(_DocSubType).GetValue(instance, null);


        //    if (DocSubType != null && value == null)
        //    {
        //        if (ErrorMsg.DocSubTypeFoFDR.Contains(DocSubType))
        //        {
        //            return new ValidationResult(ErrorMessage);
        //        }
        //    }

        //    return ValidationResult.Success;
        //}
    }

    public class FinYearRequied : ValidationAttribute
    {
        public string _DocSubType { get; set; }

        public FinYearRequied(string DocSubType, string errorMessage = "")
        {
            _DocSubType = DocSubType;
            ErrorMessage = errorMessage;
        }

        //protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        //{
        //    var instance = validationContext.ObjectInstance;
        //    var type = instance.GetType();
        //    var DocSubType = type.GetProperty(_DocSubType).GetValue(instance, null);

        //    if (DocSubType != null && value == null)
        //    {
        //        if (ErrorMsg.DocSubTypeForFinYear.Contains(DocSubType))
        //        {
        //            return new ValidationResult(ErrorMessage);
        //        }
        //    }
        //    return ValidationResult.Success;
        //}
    }

    public class QuarterRequied : ValidationAttribute
    {
        public string _DocSubType { get; set; }

        public QuarterRequied(string DocSubType, string errorMessage = "")
        {
            _DocSubType = DocSubType;
            ErrorMessage = errorMessage;
        }

        //protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        //{
        //    var instance = validationContext.ObjectInstance;
        //    var type = instance.GetType();
        //    var DocSubType = type.GetProperty(_DocSubType).GetValue(instance, null);

        //    if (DocSubType != null && value == null)
        //    {
        //        if (ErrorMsg.DocSubTypeForQuarter.Contains(DocSubType))
        //        {
        //            return new ValidationResult(ErrorMessage);
        //        }
        //    }
        //    return ValidationResult.Success;
        //}
    }
    #endregion
}

