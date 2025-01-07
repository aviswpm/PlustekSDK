using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace LibWFXDemo_CSharp
{
    /// <summary>
    /// Interaction logic for FormECO.xaml
    /// </summary>
    public partial class FormECO : Window
    {
        uint m_ultime;
        public FormECO(uint ulTime)
        {
            InitializeComponent();
            m_ultime = ulTime;
            TXT_ECO_TIME.Text = m_ultime.ToString();
        }

        public MessageBoxResult ShowDialog(out uint result)
        {
            MessageBoxResult dialogResult = MessageBoxResult.OK;
            result = m_ultime;
            return dialogResult;
        }

        private void BTN_SET_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                m_ultime = Convert.ToUInt32(TXT_ECO_TIME.Text);
                Close();
            }
            catch { }
        }
    }
}
