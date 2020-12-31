using System.Threading.Tasks;

using Amazon.Lambda.Core;

using ec2changeinstancetype.Model;
using ec2changeinstancetype.Services;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace ec2changeinstancetype
{
    public class Function
    {
        /// <summary>
        /// Default constructor. This constructor is used by Lambda to construct the instance. When invoked in a Lambda environment
        /// the AWS credentials will come from the IAM role associated with the function and the AWS region will be set to the
        /// region the Lambda function is executed in.
        /// </summary>
        public Function()
        {

        }


        /// <summary>
        /// This method is called for every Lambda invocation. This method takes in an SNS event object and can be used 
        /// to respond to SNS messages.
        /// </summary>
        /// <param name="evnt"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task FunctionHandler(Ec2InstanceStateEvent evnt, ILambdaContext context){

            LambdaLogger.Log("Inicio de cambio de tipo de instancia.");
            await Ec2Services.ModifyInstanceAsync();
            LambdaLogger.Log("Fin de cambio de tipo de instancia.");
        }
    }
}
