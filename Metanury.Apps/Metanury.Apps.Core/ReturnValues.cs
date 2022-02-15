namespace Metanury.Apps.Core
{
    public class ReturnValues<T> : ReturnValue where T : class
    {
        public T Data { get; set; }
        public ReturnValues() : base()
        {
            this.Data = default(T);
        }

        public virtual void Success(long code, T obj)
        {
            this.Code = code;
            this.Data = obj;
            this.Check = true;
        }

        public virtual void Success(long code, T obj, string value)
        {
            this.Code = code;
            this.Data = obj;
            this.Value = value;
            this.Check = true;
        }

        public virtual void Success(long code, T obj, string value, string msg)
        {
            this.Code = code;
            this.Data = obj;
            this.Value = value;
            this.Message = msg;
            this.Check = true;
        }
    }
}
