﻿namespace Orc.Controls
{
    using System;
    using Catel.Data;

    public abstract class ControlToolBase : ModelBase, IControlTool
    {
        protected object Target;

        public abstract string Name { get; }
        public bool IsOpened { get; private set; }
        public virtual bool IsEnabled => true;

        public virtual void Attach(object target)
        {
            Target = target;

            Attached?.Invoke(this, EventArgs.Empty);
        }

        public virtual void Detach()
        {
            Target = null;

            Detached?.Invoke(this, EventArgs.Empty);
        }

        public void Open(object parameter = null)
        {
            if (IsOpened)
            {
                OnAddParameter(parameter);

                return;
            }

            Opening?.Invoke(this, EventArgs.Empty);

            OnOpen(parameter);

            IsOpened = true;
            Opened?.Invoke(this, EventArgs.Empty);
        }

        public virtual void Close()
        {
            IsOpened = false;

            Closed?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler<EventArgs> Attached;
        public event EventHandler<EventArgs> Detached;
        public event EventHandler<EventArgs> Closed;
        public event EventHandler<EventArgs> Opened;
        public event EventHandler<EventArgs> Opening;

        protected virtual void OnAddParameter(object? parameter)
        {
        }

        protected abstract void OnOpen(object? parameter = null);
    }
}
