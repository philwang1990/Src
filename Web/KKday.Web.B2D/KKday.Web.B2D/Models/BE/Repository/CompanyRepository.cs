using System;
using System.Collections.Generic;
using KKday.Web.B2D.BE.AppCode.DAL.Company;
using KKday.Web.B2D.BE.Models.Model.Company;
using KKday.Web.B2D.BE.Models.Model.Common;
using Newtonsoft.Json.Linq;
using Resources;
using KKday.Web.B2D.BE.Areas.KKday.Models.DataModel;
using KKday.Web.B2D.BE.Models.Model.PriceSetting;

namespace KKday.Web.B2D.BE.Models.Repository
{
    public class CompanyRepository
    {
        private readonly ILocalizer _localizer;

        public CompanyRepository(ILocalizer localizer)
        {
            _localizer = localizer;
        }

        // 取得 QueryParamModel
        public QueryParamsModel GetQueryParamModel(string filter, string sorting, int size, int current_page)
        {
            var rec_count = GetCompanyCount(filter);
            var total_pages = (int)(rec_count / size) + ((rec_count % size != 0) ? 1 : 0);
            return new QueryParamsModel()
            {
                Filter = filter,
                Sorting = sorting,
                Paging = new Pagination()
                {
                    current_page = current_page,
                    total_count = rec_count,
                    total_pages = total_pages,
                    page_size = size
                }
            };
        }

        public int GetCompanyCount(string filter)
        {
            try
            {
                var _filter = GetFieldFiltering(filter);
                return CompanyDAL.GetCompanyCount(_filter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<B2dCompany> GetCompanies(string filter, int skip, int size, string sorting)
        {
            var _filter = GetFieldFiltering(filter);
            var _sorting = GetFieldSorting(sorting);

            var company_list = CompanyDAL.GetCompanies(_filter, skip, size, _sorting);
            company_list.ForEach(c => {
                //狀態語系文字對應
                c.STATUS_DESC = GetFieldStatus(c.STATUS);
            });

            return company_list;
        }

        public B2dCompany GetCompany(Int64 xid)
        {
            var comp = CompanyDAL.GetCompany(xid);
            //狀態語系文字對應
            comp.STATUS_DESC = GetFieldStatus(comp.STATUS);

            return comp;
        }

        public void Update(CompanyUpdModel company, string upd_user)
        {
            CompanyDAL.UpdateCompany(company, upd_user);
        }

        public void UpdateLicenses(Int64 xid, string[] license_url, string upd_user)
        {
            CompanyDAL.UpdateLicenses(xid, license_url, upd_user);
        }

        public void SetStatus(Int64 xid, string status, string upd_user)
        {
            CompanyDAL.UpdateStatus(xid, status, upd_user);
        }

        public List<B2dDiscountMst> GetDiscounts(Int64 company_xid)
        {
            return CompanyDiscountDAL.GetDiscountMst(company_xid);
        }

        public List<B2dDiscountMst> GetAvailableDiscounts(Int64 company_xid)
        {
            return CompanyDiscountDAL.GetAvailableDiscountMst(company_xid);
        }

        public void InsertDiscount(Int64 company_xid, Int64[] items, string crt_user)
        {
            CompanyDiscountDAL.InsertDiscount(company_xid, items, crt_user);
        }

        public void RemoveDiscount(Int64 company_xid, Int64 mst_xid, string del_user)
        {
            CompanyDiscountDAL.RemoveDiscount(company_xid, mst_xid, del_user);
        }

        #region Fields Mapping

        private string GetFieldFiltering(string filter)
        {
            var _filter = "";
            var jObjFilter = string.IsNullOrEmpty(filter) ? new JObject() : JObject.Parse(filter);

            if (!string.IsNullOrEmpty((string)jObjFilter["name"]))
                _filter += $" AND comp_name LIKE '%{jObjFilter["name"]}%' ";

            if (!string.IsNullOrEmpty((string)jObjFilter["country"]))
                _filter += $" AND comp_country = '{jObjFilter["country"]}' ";

            if (!string.IsNullOrEmpty((string)jObjFilter["status"]))
                _filter += $" AND status = '{jObjFilter["status"]}' ";


            return _filter;
        }

        private string GetFieldSorting(string sorting)
        {
            var _sorting = "";

            switch (sorting)
            {

                case "name": _sorting = "comp_name"; break;
                case "status": _sorting = "status"; break;
                case "xid": _sorting = "xid"; break;
                case "country": _sorting = "comp_country"; break;
                default: break;
            }

            return _sorting;
        }

        // 狀態對應
        private string GetFieldStatus(string status)
        {
            var _strDesc = "";

            switch (status)
            {
                case "00": _strDesc = _localizer.Text.Company_Status_00; break;
                case "01": _strDesc = _localizer.Text.Company_Status_01; break;
                case "02": _strDesc = _localizer.Text.Company_Status_02; break;
                case "03": _strDesc = _localizer.Text.Company_Status_03; break;
                case "04": _strDesc = _localizer.Text.Company_Status_04; break;
                default: break;
            }
            return _strDesc;
        }

        #endregion


    }
}
