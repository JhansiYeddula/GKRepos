using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace GKCustomAddress.Helper
{
   public class CrmDataHelper
    {
     

            public EntityCollection GetCustomerAddress(IOrganizationService service ,Guid parentid)
        {

            //            var fetchXml = $@"
            //<fetch>
            //  <entity name='customeraddress'>
            //    <all-attributes />
            //    <link-entity name='contact' from='contactid' to='parentid' link-type='inner' alias='a'>
            //      <filter type='and'>
            //        <condition attribute='contactid' operator='eq' value='"+parentid+"'/></filter></link-entity></entity></fetch>";



            //            EntityCollection entityCollection = service.RetrieveMultiple(new FetchExpression(fetchXml));
            FilterExpression filter1 = new FilterExpression();
            ConditionExpression conditionParentId = new ConditionExpression();

            conditionParentId.AttributeName = AddressAttributes.parentid;
            conditionParentId.Operator = ConditionOperator.Equal;
            conditionParentId.Values.Add(parentid);
            filter1.Conditions.Add(conditionParentId);
            QueryExpression queryExpression = new QueryExpression("customeraddress");
            queryExpression.ColumnSet = new ColumnSet(true);
            //queryExpression.ColumnSet = new ColumnSet("name","postalcode", "telephone1", "country", "addresstypecode", "addressnumber", "parentid");
            queryExpression.Criteria.AddFilter(filter1);
            EntityCollection entityCollection = service.RetrieveMultiple(queryExpression);
            return entityCollection;
        }

        public int GetMaxAddressNumber(IOrganizationService service, ITracingService tracingService,Guid parentId,int objectTypeCode)
        {
            int maxAddressNumber = 0;
            string fetchXml = string.Empty;
            //if (objectTypeCode == 2)
            //{
                fetchXml = @"<fetch distinct='false' mapping='logical' aggregate='true'>   
             <entity name='poad_customaddress'>   
             <attribute name='poad_addressnumber' aggregate='max' alias='maxAddressNumber'/> 
            <filter type='and'>
             <condition attribute='poad_customerid' operator='eq' value='" + parentId + "'/></filter></entity></fetch>";
            //}
            //else
            //{
            //    fetchXml = @"<fetch distinct='false' mapping='logical' aggregate='true'>   
            // <entity name='poad_customaddress'>   
            // <attribute name='poad_addressnumber' aggregate='max' alias='maxAddressNumber'/> 
            //<filter type='and'>
            // <condition attribute='poad_accountid' operator='eq' value='" + parentId + "'/></filter></entity></fetch>";
            //}
            EntityCollection maxBatch_result = service.RetrieveMultiple(new FetchExpression(fetchXml));

            foreach (var c in maxBatch_result.Entities)
            {
                maxAddressNumber = ((int)((AliasedValue)c["maxAddressNumber"]).Value);
                tracingService.Trace("Max Address Number value: " + maxAddressNumber);

            }
            if(maxAddressNumber==0)
            {
                fetchXml = @"<fetch distinct='false' mapping='logical' aggregate='true'>   
             <entity name='customeraddress'>   
             <attribute name='addressnumber' aggregate='max' alias='maxAddressNumber'/> 
            <filter type='and'>
             <condition attribute='parentid' operator='eq' value='" + parentId + "'/></filter></entity></fetch>";

                EntityCollection maxBatch_result1 = service.RetrieveMultiple(new FetchExpression(fetchXml));

                foreach (var c in maxBatch_result1.Entities)
                {
                    maxAddressNumber = ((int)((AliasedValue)c["maxAddressNumber"]).Value);
                    tracingService.Trace("Max Address Number from address entity value: " + maxAddressNumber);

                }
            }
           
            return maxAddressNumber + 1;
        }
        public Guid GetAddressId(IOrganizationService service, int addressnumber,Guid parentId)
        {

            Guid addressId = Guid.Empty;
            FilterExpression filter1 = new FilterExpression();
            ConditionExpression conditionParentId = new ConditionExpression();
           
                conditionParentId.AttributeName = AddressAttributes.parentid;
           
            conditionParentId.Operator = ConditionOperator.Equal;
            conditionParentId.Values.Add(parentId);
            filter1.Conditions.Add(conditionParentId);


            ConditionExpression conditionAddressNumber = new ConditionExpression();
            conditionAddressNumber.AttributeName = AddressAttributes.addressnumber;
            conditionAddressNumber.Operator = ConditionOperator.Equal;
            conditionAddressNumber.Values.Add(addressnumber);
            filter1.Conditions.Add(conditionAddressNumber);



            QueryExpression queryExpression = new QueryExpression("customeraddress");
            queryExpression.ColumnSet = new ColumnSet("customeraddressid");
            queryExpression.Criteria.AddFilter(filter1);
            EntityCollection entityCollection = service.RetrieveMultiple(queryExpression);
            if (entityCollection != null && entityCollection.Entities.Count > 0)
                addressId = entityCollection.Entities[0].Id;

            return addressId;
        }
        public Guid GetParentIdCustomerAddress(IOrganizationService service, int addressnumber,Guid parentId,int objecttypecode)
        {
            Guid customerAddressId = Guid.Empty;
            FilterExpression filter1 = new FilterExpression();
            ConditionExpression conditionParentId = new ConditionExpression();
            //if (objecttypecode == 1)
            //{
            //     conditionParentId.AttributeName = CustomAddressAttributes.poad_accountid;
            //}
            //else if(objecttypecode ==2 )
            //{
            //    conditionParentId.AttributeName = CustomAddressAttributes.poad_contactid;
            //}
            conditionParentId.AttributeName = CustomAddressAttributes.poad_customerid;
            conditionParentId.Operator = ConditionOperator.Equal;
            conditionParentId.Values.Add(parentId);
            filter1.Conditions.Add(conditionParentId);


            ConditionExpression conditionAddressNumber = new ConditionExpression();
            conditionAddressNumber.AttributeName = CustomAddressAttributes.poad_addressnumber;
            conditionAddressNumber.Operator = ConditionOperator.Equal;
            conditionAddressNumber.Values.Add(addressnumber);
            filter1.Conditions.Add(conditionAddressNumber);

            

            QueryExpression queryExpression = new QueryExpression("poad_customaddress");
            queryExpression.ColumnSet = new ColumnSet("poad_customaddressid");
            queryExpression.Criteria.AddFilter(filter1);
            EntityCollection entityCollection = service.RetrieveMultiple(queryExpression);
            if (entityCollection != null && entityCollection.Entities.Count > 0)
                customerAddressId = entityCollection.Entities[0].Id;

            return customerAddressId;
        }
    }
}
