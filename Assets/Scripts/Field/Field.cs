using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    [SerializeField] private FieldComponent fundo;
    [SerializeField] private FieldComponent grades;
    [SerializeField] private FieldComponent gramado;
    [SerializeField] private FieldComponent linhas_baixo;
    [SerializeField] private FieldComponent linhas_cima;

    [SerializeField] private Transform goal;
    
    public void SetGoalPosition(float startGoalY, float endGoalY)
    {
        float centerY = (endGoalY + startGoalY) / 2f;
        transform.position = new Vector3(transform.position.x, centerY);
        
        fundo.SetGoalPosition(startGoalY, endGoalY);
        grades.SetGoalPosition(startGoalY, endGoalY);
        gramado.SetGoalPosition(startGoalY, endGoalY);
        linhas_baixo.SetGoalPosition(endGoalY-centerY);
        linhas_cima.SetGoalPosition(endGoalY-centerY);
        goal.position = new Vector3(goal.position.x, startGoalY);
    }
}
