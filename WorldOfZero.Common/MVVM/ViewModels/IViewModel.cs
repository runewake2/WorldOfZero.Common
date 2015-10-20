using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WorldOfZero.Common.MVVM.ViewModels
{
    public interface IViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Notifies listener that the value of the specified property has changed.
        /// </summary>
        void NotifyPropertyChanged([CallerMemberName] string propertyName = null);
    }
}
