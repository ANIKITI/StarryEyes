﻿using System;
using System.Reactive.Linq;
using StarryEyes.Albireo.Collections;
using StarryEyes.Anomaly.TwitterApi.DataModels;
using StarryEyes.Anomaly.TwitterApi.Rest;
using StarryEyes.Anomaly.Utils;
using StarryEyes.Models.Receiving;
using StarryEyes.Settings;

namespace StarryEyes.Filters.Sources
{
    public class FilterSearch : FilterSourceBase
    {
        private readonly AVLTree<long> _acceptIds = new AVLTree<long>();
        private readonly string _query;
        public FilterSearch(string query)
        {
            this._query = query;
        }

        public override Func<TwitterStatus, bool> GetEvaluator()
        {
            // accept all status via web.
            return status =>
            {
                lock (_acceptIds)
                {
                    return _acceptIds.Contains(status.Id);
                }
            };
        }

        public override string GetSqlQuery()
        {
            return "0"; // always return 'false'
        }

        protected override IObservable<TwitterStatus> ReceiveSink(long? maxId)
        {
            return Observable.Start(() => Setting.Accounts.GetRandomOne())
                             .Where(a => a != null)
                             .SelectMany(a => a.SearchAsync(_query, maxId: maxId).ToObservable())
                             .Do(s =>
                             {
                                 lock (_acceptIds)
                                 {
                                     _acceptIds.Add(s.Id);
                                 }
                             });
        }

        private bool _isActivated;
        public override void Activate()
        {
            if (_isActivated) return;
            _isActivated = true;
            ReceiveManager.RegisterSearchQuery(_query, _acceptIds);
        }

        public override void Deactivate()
        {
            if (!_isActivated) return;
            _isActivated = false;
            ReceiveManager.UnregisterSearchQuery(_query, _acceptIds);
        }

        public override string FilterKey
        {
            get { return "search"; }
        }

        public override string FilterValue
        {
            get { return _query; }
        }

    }
}
