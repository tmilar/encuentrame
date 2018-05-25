using System.Collections.Generic;

namespace Encuentrame.Model.Businesses
{
    public interface IBusinessCommand
    {

        Business Get(int id);
      
        void Create(BusinessCommand.CreateOrEditParameters parameters);

        IList<Business> List();
        void Edit(int id, BusinessCommand.CreateOrEditParameters parameters);
        void Delete(int id);
    }
}