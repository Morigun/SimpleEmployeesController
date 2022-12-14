
using SimpleEmployeesController.Models;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SimpleEmployeesController.MVVM
{
    public abstract class ObservableObject : INotifyPropertyChanged, IDisposable
    {
        /// <summary>
        /// Метод для оповещения подписчиков о изменении поля объекта
        /// </summary>
        /// <typeparam name="T">Тип изменяемого объекта</typeparam>
        /// <param name="field">Поле куда занести новое значение</param>
        /// <param name="value">Значение</param>
        /// <param name="propertyName">Имя изменяемого свойства</param>
        protected void Set<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return;

            field = value;
            if (propertyName != null)
                OnPropertyChanged(propertyName);
        }
        #region Notification
        public event PropertyChangedEventHandler? PropertyChanged;
        public virtual void OnNotification() { }

        public virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            OnNotification();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void NotifyPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }
        #endregion
        #region Dispose
        private bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Метод для освобождения памяти в дочерних типах
        /// </summary>
        protected abstract void FreeObjects();
        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;
            if (disposing)
            {
                FreeObjects();
            }
            disposed = true;
        }
        #endregion
    }
}
