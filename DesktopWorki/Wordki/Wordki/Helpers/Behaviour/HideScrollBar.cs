using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace Wordki.Helpers
{
    public class HideScrollBar : Behavior<DataGrid>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            if (AssociatedObject == null)
            {
                return;
            }
            AssociatedObject.MouseEnter += OnMouseEnter;
            AssociatedObject.MouseLeave += OnMouseLeave;
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            Console.WriteLine("Leave");
        }

        private void OnMouseEnter(object sender, MouseEventArgs e)
        {
            Console.WriteLine("Enter");
        }

        protected override void OnDetaching()
        {
            if (AssociatedObject == null)
            {
                return;
            }
            base.OnDetaching();
            AssociatedObject.MouseEnter -= OnMouseEnter;
            AssociatedObject.MouseLeave -= OnMouseLeave;
        }
    }
}
