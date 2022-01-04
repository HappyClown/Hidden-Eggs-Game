using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class QueueSequenceManager : MonoBehaviour
{
    Queue<SequenceRequest> sequenceRequestQueue = new Queue<SequenceRequest>();
    SequenceRequest currentSequenceRequest;
    static QueueSequenceManager instance;
    bool inASequence;
    
    void Awake() {
        instance = this;
    }

    public static void AddSequenceToQueue(Action callback) {
        SequenceRequest newRequest = new SequenceRequest(callback);
        instance.sequenceRequestQueue.Enqueue(newRequest);
        instance.TryProcessNext();
    }

    void TryProcessNext() {
        if (!inASequence && sequenceRequestQueue.Count > 0) {
            currentSequenceRequest = sequenceRequestQueue.Dequeue();
            inASequence = true;
            currentSequenceRequest.callback();
        }
    }

    public static void SequenceComplete() {
        instance.inASequence = false;
        instance.TryProcessNext();
    }

    struct SequenceRequest {
        public Action callback;
        public SequenceRequest(Action _callback) {
            callback = _callback;
        }
    }
}
