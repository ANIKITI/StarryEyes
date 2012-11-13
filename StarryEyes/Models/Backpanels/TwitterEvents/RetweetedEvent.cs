﻿using StarryEyes.Breezy.DataModel;
using StarryEyes.Views;

namespace StarryEyes.Models.Backpanels.TwitterEvents
{
    public sealed class RetweetedEvent : TwitterEventBase
    {
        public RetweetedEvent(TwitterUser source, TwitterStatus target)
            :base(source, target){}

        public override string Title
        {
            get { return "RT"; }
        }

        public override string Detail
        {
            get { return Source.ScreenName + ": " + TargetStatus.ToString(); }
        }

        public override System.Windows.Media.Color Background
        {
            get
            {
                return MetroColors.Green;
            }
        }
    }
}