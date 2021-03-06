﻿using StarryEyes.Views;

namespace StarryEyes.Models.Backstages.NotificationEvents
{
    internal sealed class InternalErrorEvent : BackstageEventBase
    {
        private readonly string _description;

        public InternalErrorEvent(string description)
        {
            this._description = description;
        }

        public override string Title
        {
            get { return "ERROR"; }
        }

        public override string Detail
        {
            get { return _description; }
        }

        public override System.Windows.Media.Color Background
        {
            get { return MetroColors.Red; }
        }
    }
}