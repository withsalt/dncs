using DNCS.Data.Model.Common.JsonObjectNode;
using WithSalt.Common.Date;

namespace DNCS.Data.Model.Common.ResultInfo
{
    public class ResultModel<T> : IRoot<T> where T : IChild
    {
        public ResultModel()
        {

        }

        public ResultModel(int code)
        {
            this.Code = code;
            this.Message = "Success";
        }

        public ResultModel(int code, string message)
        {
            this.Code = code;
            this.Message = message;
        }

        public int Code { get; set; }

        public string Message { get; set; }

        public T Data { get; set; }

        public int Time
        {
            get
            {
                return TimeUtil.Timestamp();
            }
            set
            {

            }
        }

        public int Check
        {
            get
            {
                return 5;
            }
            set
            {

            }
        }
    }
}
