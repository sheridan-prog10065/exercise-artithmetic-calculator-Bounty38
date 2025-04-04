﻿using System.Collections.ObjectModel;
using System.Diagnostics;

namespace MathOperators;

public partial class MainPage : ContentPage
{
    
    //Remembers calculations don by the user.
    private ObservableCollection<string> _expList;
    public MainPage()
    {
        _expList = new ObservableCollection<string>();
        
        InitializeComponent();
        
        _lstExpHistory.ItemsSource = _expList;
    }

    //OnCalculate event for when the "Calculate" button is pressed.
    private async void OnCalculate(object sender, EventArgs e)
    {
        try
        {
            //Get input to the Arithmetic Operation.
            double leftOperand = ReadLeftOperand();
            double rightOperand = ReadRightOperand();

            //Obtain the character that represents the operation
            //Cast to string is possible because SelectedItem is an object.
            //Extra parenthesis are needed to ensure the index operator is applied to the result. 
            char operation = ReadArithmeticOperation();

            //Perform the Arithmetic Operation and obtain the result.
            double result = PerformOperation(operation, leftOperand, rightOperand);

            //Display the result to the user and save to history.
            string expression = $"{leftOperand} {operation} {rightOperand} = {result}";
            _expList.Add(expression);

            _txtMathExp.Text = expression;

            _lstExpHistory.ItemsSource = null;
            _lstExpHistory.ItemsSource = _expList;
        }
        
        catch (CalculatorException ex)
        {
            //The user tried to divide by 0.
            await DisplayAlert(title: "Arithmetic Calculator", ex.Message, cancel: "OK");
        }

    }

    private char ReadArithmeticOperation()
    {
        //Step 1: Obtain the input
        string opInput = _pckOperand.SelectedItem as string;

        //Step 2: Validate the input
        if (String.IsNullOrWhiteSpace(opInput))
        {
            throw new CalculatorException("Please select one of the arithmetic operations");
        }

        //Step 3: Use the input
        //Obtain the character that represents the operation
        //Cast to string is possible because SelectedItem is an object.
        //Extra parenthesis are needed to ensure the index operator is applied to the result.
        char operation = ((string)_pckOperand.SelectedItem)[0];

        return operation;
    }

    private double ReadRightOperand()
    {
        return double.Parse(_txtRightOp.Text);
    }

    private double ReadLeftOperand()
    {
        return double.Parse(_txtLeftOp.Text);
    }

    private double PerformOperation(char operation, double leftOperand, double rightOperand)
    {
        //Perform the calculation.
        switch (operation)
        {
            case '+':
                return PerformAddition(leftOperand, rightOperand);
            
            case '-':
                return PerformSubtraction(leftOperand, rightOperand);
            
            case '*':
                return PerformMultiplication(leftOperand, rightOperand);
            
            case '/':
                return PerformDivision(leftOperand, rightOperand);
            
            case '%':
                return PerformDivRemainder(leftOperand, rightOperand);
            
            default:
                Debug.Assert(false, "Unknown arithmetic operand. Cannot perform operation.");
                return 0;
        }

        

    }
    private double PerformAddition(double leftOperand, double rightOperand)
    {
        return leftOperand + rightOperand;
    }
    
    private double PerformSubtraction(double leftOperand, double rightOperand)
    {
        return leftOperand - rightOperand;
    }
    
    private double PerformMultiplication(double leftOperand, double rightOperand)
    {
        return leftOperand * rightOperand;
    }
    
    private double PerformDivision(double leftOperand, double rightOperand)
    {
        string divOp = _pckOperand.SelectedItem as string;
        if (divOp.Contains("Int", StringComparison.OrdinalIgnoreCase))
        {
            //Integer Division
            int intLeftOp = (int)leftOperand;
            int intRightOp = (int)rightOperand;
            int result = intLeftOp / intRightOp;
            return result;
        }
        else
        {
            //Real Division
            return leftOperand / rightOperand;
        }
    }
    
    private double PerformDivRemainder(double leftOperand, double rightOperand)
    {
        return leftOperand % rightOperand;
    }
    
}