namespace AuroraBricks.Models;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms.Onnx;

public class OnnxOutput
{
        [ColumnName("Predicted_Fraud")]
        public double PredictedFraud { get; set; }
}