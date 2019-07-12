using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Xamarin.DataAnnotationsReflection
{
    public class MyViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private string _myTextEntry;
        [MaxLength(10)]
        public string MyTextEntry
        {
            get { return _myTextEntry; }
            set
            {
                if (_myTextEntry == value)
                    return;

                _myTextEntry = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MyTextEntry)));
                System.Diagnostics.Debug.WriteLine($"My Entry Text Changed to: {value}");
                ValidateModel();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void ValidateModel()
        {
            _validationErrors.Clear();
            var context = new ValidationContext(this);
            var valid = Validator.TryValidateObject(this, context, _validationErrors, true);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HasErrors)));
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(MyTextEntry)));
            System.Diagnostics.Debug.WriteLine($"Validation has been completed: {valid}, errors: {_validationErrors.Count}");
        }

        private readonly List<ValidationResult> _validationErrors = new List<ValidationResult>();

        public bool HasErrors => _validationErrors.Count > 0;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            var errors = new List<string>();
            if (HasErrors)
            {
                errors = _validationErrors
                    .Where(e => e.MemberNames.Contains(propertyName))
                    .Select(e => e.ErrorMessage)
                    .ToList();
            }

            return errors;
        }
    }
}
