using System.Drawing;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms.Onnx;


namespace AuroraBricks.Models
{
    public class OnnxInput
    {
            [ColumnName("time")]
            [OnnxMapType(typeof(Int64), typeof(Single))]
            public double time { get; set; }

            [ColumnName("amount")]
            [OnnxMapType(typeof(Int64),typeof(Single))]
            public double amount { get; set; }

            [ColumnName("country_of_transaction_United Kingdom")]
            [OnnxMapType(typeof(Int64), typeof(Single))]
            public double contryoftransactionUnitedKingdom { get; set; }


        

        // public BrixOrder Orders { get; set; }
        //
        // public string Prediction {  get; set; }
    }

}
