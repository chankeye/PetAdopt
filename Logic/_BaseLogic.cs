using NLog;
using PetAdopt.DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using PetAdopt.Models;

namespace PetAdopt.Logic
{
    public abstract class _BaseLogic
    {
        /// <summary>
        /// Base的Logic，所有Logic都要繼承他
        /// </summary>
        /// <param name="operation">操作者資訊</param>
        public _BaseLogic(Operation operation)
        {
            GetLogger().Debug("Display: {0}, PersonId: {1}", operation.Display, operation.UserId);

            _operation = operation;
        }

        /// <summary>
        /// PetContext
        /// </summary>
        protected PetContext PetContext
        {
            get { return _petContext.Value; }
        }
        Lazy<PetContext> _petContext = new Lazy<PetContext>();

        /// <summary>
        /// 取得一個新的Logger物件
        /// </summary>
        protected Logger GetLogger()
        {
            var method = new StackFrame(1).GetMethod();
            var fullMethodName =
                string.Format(
                    "{0}.{1}",
                    method.DeclaringType.FullName,
                    method.Name
                );

            return LogManager.GetLogger(fullMethodName);
        }
        /// <summary>
        /// 取得一個加上特定前置詞的Logger物件
        /// (比平常的GetLogger()固定多call一層,logic多一個GetLogger,各method寫法可不變)
        /// </summary>
        /// <param name="prefix">前置詞</param>
        /// <returns></returns>
        protected Logger GetLogger(string prefix)
        {
            var method = new StackFrame(1).GetMethod();
            var fullMethodName =
                string.Format(
                    "{0}.{1}",
                    method.DeclaringType.FullName,
                    method.Name
                );
            var loggerName = string.Format("{0}{1}",
                    string.IsNullOrWhiteSpace(prefix) ? string.Empty : prefix + "."
                    , fullMethodName
                );

            return LogManager.GetLogger(loggerName);
        }

        /// <summary>
        /// 取得一個新的操作資訊
        /// </summary>
        /// <returns></returns>
        protected OperationInfo GetOperationInfo()
        {
            return new OperationInfo
            {
                Date = DateTime.UtcNow,
                UserId = _operation.UserId
            };
        }
        Operation _operation;

        /// <summary>取得exception所有inner exception的type</summary>
        protected IEnumerable<Type> GetAllExceptionType(Exception ex)
        {
            //只要他所有的type就好. 並不需要exception本身
            List<Type> list = new List<Type>();
            //assign the current exception as first object and then loop through its
            //inner exceptions till they are null
            for (Exception eCurrent = ex; eCurrent != null; eCurrent = eCurrent.InnerException)
            {
                list.Add(eCurrent.GetType());
            }
            return list;
        }
    }
}