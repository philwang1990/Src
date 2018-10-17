using System;
using System.Collections.Generic;
using KKday.Web.B2D.BE.AppCode.DAL.Company;
using KKday.Web.B2D.BE.Models.Company;

namespace KKday.Web.B2D.BE.Models.Repository
{
    public class CompanyRepository
    {
      
        public int GetCompanyCount(string filter)
        {
            try
            {
                return ComapnyDAL.GetCompanyCount(filter);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public List<B2D_Company> GetCompanies(string filter, int skip, int size, string sorting)
        {
            return ComapnyDAL.GetCompanies(filter, skip, size, sorting);
        }
    }
}
