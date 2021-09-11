using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace SpottedTrains
{
    /// <summary>
    /// Interaction logic for MyTextbox.xaml
    /// </summary>
    public partial class MyTextbox : UserControl
    {

        private ObservableCollection<ValidationRule> _validationRules = null;

        public MyTextbox()
        {
            InitializeComponent();
            CreateControls();

            ValidationRules = new ObservableCollection<ValidationRule>();
            DataContextChanged += new DependencyPropertyChangedEventHandler(RequiredTextBox_DataContextChanged);
        }

        public ObservableCollection<ValidationRule> ValidationRules
        {
            get { return _validationRules; }
            set { _validationRules = value; }
        }

        private void CreateControls()
        {
            TheTextBox.LostFocus += _textBox_LostFocus;
            TheTextBox.Style = TextBoxErrorStyle;
        }

        void _textBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var bindingExpression = TheTextBox.GetBindingExpression(TextBox.TextProperty);
            if (bindingExpression != null)
                bindingExpression.UpdateSource();
        }

        void RequiredTextBox_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (TheTextBox != null)
            {
                var binding = new Binding();
                binding.Source = DataContext;
                binding.ValidatesOnDataErrors = true;
                binding.ValidatesOnExceptions = true;

                foreach (var rule in ValidationRules)
                {
                    binding.ValidationRules.Add(rule);
                }


                binding.Path = new PropertyPath(BoundPropertyName);
                TheTextBox.SetBinding(TextBox.TextProperty, binding);
            }

        }

        private Style TextBoxErrorStyle
        {
            get
            {
                return Application.Current.TryFindResource("TextBoxStyle") as Style;
            }
        }

        public string TextBoxErrorStyleName { get; set; }
        public string ValidationExpression { get; set; }
        public string BoundPropertyName { get; set; }
        public string Text
        {

            get
            {
                return TheTextBox.Text;
            }

        }
        public string ErrorText { get; set; }
    }
}
