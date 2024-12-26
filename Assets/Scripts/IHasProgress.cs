using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IHasProgress
{
    /// <summary>
    /// Event handler for progress bar
    /// </summary>
    public event EventHandler<OnProgressChangedEventArgs> OnProgressChanged;

    /// <summary>
    /// Custom event args
    /// </summary>
    public class OnProgressChangedEventArgs : EventArgs
    {
        public float progressNormalized;
    }
    
}
