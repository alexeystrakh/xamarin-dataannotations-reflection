using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace Xamarin.DataAnnotationsReflection
{
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void Submit_Click(object sender, EventArgs e)
        {
            var model = this.BindingContext;
            var props = model.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var validationProps = props.Select(p => new { Property = p, ValidationAttribute = p.GetCustomAttribute<ValidationAttribute>() })
                .Where(p => p.ValidationAttribute != null)
                .ToList();

            System.Diagnostics.Debug.WriteLine($"Found {validationProps.Count} validation properties: {string.Join(",", validationProps.Select(vp => vp.Property.Name))}");
        }
    }
}
