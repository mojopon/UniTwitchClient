using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace UniTwitchClient.Tests.EventSub
{
    public class SubjectComposition : IDisposable
    {
        private CompositeDisposable _disposables = new CompositeDisposable();
        private Dictionary<string, object> _subjectDict = new Dictionary<string, object>();
        private IDictionary<string, IDisposable> _disposeDict = new Dictionary<string, IDisposable>();
        private IDictionary<string, Action<object>> _onNextDict = new ConcurrentDictionary<string, Action<object>>();
        private IDictionary<string, Action> _onCompletedDict = new ConcurrentDictionary<string, Action>();
        private IDictionary<string, Action<Exception>> _onErrorDict = new ConcurrentDictionary<string, Action<Exception>>();

        public Subject<T> CreateSubject<T>(string key) where T : class
        {
            if (!_subjectDict.ContainsKey(key))
            {
                var subject = new Subject<T>().AddTo(_disposables);
                _subjectDict.Add(key, subject);
                _disposeDict.Add(key, subject);

                Action<object> onNextLambda = (obj) => subject.OnNext((T)obj);
                _onNextDict.Add(key, onNextLambda);

                Action onCompletedLambda = () => subject.OnCompleted();
                _onCompletedDict.Add(key, onCompletedLambda);

                Action<Exception> onErrorLambda = ex => subject.OnError(ex);
                _onErrorDict.Add(key, onErrorLambda);
            }

            return (Subject<T>)_subjectDict[key];
        }

        public void OnNext(string key, object obj)
        {
            if (obj == null) { return; }

            if (_onNextDict.ContainsKey(key))
            {
                _onNextDict[key].Invoke(obj);
            }
        }

        public void OnCompleted(string key)
        {
            if (_onCompletedDict.ContainsKey(key))
            {
                _onCompletedDict[key].Invoke();
            }
        }

        public void OnError(string key, Exception exception)
        {
            if (_onErrorDict.ContainsKey(key))
            {
                _onErrorDict[key].Invoke(exception);
            }
        }

        public void DisposeSubject(string key)
        {
            if (_disposeDict.ContainsKey(key))
            {
                _disposeDict[key].Dispose();
            }
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}