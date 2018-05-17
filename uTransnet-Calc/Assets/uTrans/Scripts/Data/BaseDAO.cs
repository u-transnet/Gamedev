namespace uTrans.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public class BaseDAO<T> where T : BaseDTO, new()
    {

        private DBConnection dataService;

        public BaseDAO(DBConnection dataService)
        {
            this.dataService = dataService;
        }

        public void CreateTable()
        {
            dataService.CreateTable<T>();
        }

        public IEnumerable<T> All()
        {
            return dataService.GetAll<T>();
        }

        public T GetById(int id)
        {
            return dataService.GetById<T>(id);
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> clause)
        {
            return dataService.Find<T>(clause);
        }

        public void Insert(T dto)
        {
            dataService.Insert(dto);
        }

        public void Update(T dto)
        {
            dataService.Update(dto);
        }

        public void Delete(T dto)
        {
            dataService.Delete(dto);
        }

        public void Delete(int id)
        {
            dataService.Delete<T>(id);
        }
    }
}
