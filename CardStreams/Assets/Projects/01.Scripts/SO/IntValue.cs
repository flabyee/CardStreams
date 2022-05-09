using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "IntValueSO", menuName = "ScriptableObject/IntValue")]
public class IntValue : ScriptableObject, ISerializationCallbackReceiver
{
	public int InitialValue;
	public int InitialMaxValue;

	[NonSerialized]
	public int RuntimeValue;
	[NonSerialized]
	public int RuntimeMaxValue;

	public void OnAfterDeserialize()
	{
		RuntimeValue = InitialValue;
		RuntimeMaxValue = InitialMaxValue;
	}

	public void OnBeforeSerialize() { }
}