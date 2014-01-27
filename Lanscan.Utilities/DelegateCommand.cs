//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.Utilities
{
    using System;

    public sealed class DelegateCommand : DelegateCommand<object>
    {
        public DelegateCommand(Action executeAction)
            : this(executeAction, null)
        {
        }

        public DelegateCommand(Action executeAction, Func<bool> canExecuteFunc)
            : base(WrapExecuteAction(executeAction), WrapCanExecuteFunc(canExecuteFunc))
        {
        }

        public bool CanExecute()
        {
            var result = CanExecute(null);
            return result;
        }

        public void Execute()
        {
            Execute(null);
        }

        private static Action<object> WrapExecuteAction(Action executeAction)
        {
            if (executeAction == null)
            {
                throw new ArgumentNullException("executeAction");
            }

            return x => executeAction();
        }

        private static Predicate<object> WrapCanExecuteFunc(Func<bool> canExecuteFunc)
        {
            if (canExecuteFunc == null)
            {
                return null;
            }

            return x => canExecuteFunc();
        }
    }
}
