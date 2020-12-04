using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Microsoft.Xrm.Sdk;
using GKCustomAddress.Helper;
namespace GKCustomAddress
{
    public class HandleOutOfBoxAddress:IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {


            ITracingService tracingService =
 (ITracingService)serviceProvider.GetService(typeof(ITracingService));


            IPluginExecutionContext context = (IPluginExecutionContext)
                serviceProvider.GetService(typeof(IPluginExecutionContext));



            tracingService.Trace("Entered in R");

            if (context != null)
            {
                //tracingService.Trace(context)
            }
            if(context.Depth>1)
            {
                tracingService.Trace("depth");
                return;
            }
            //Entity entity = context.PostEntityImages["PostImage"];
            if (context.PostEntityImages.Contains("PostImage") &&
                context.PostEntityImages["PostImage"] is Entity)
            {

                Entity entity = (Entity)context.PostEntityImages["PostImage"];
                if (entity.LogicalName != "poad_customaddress")
                    return;
                IOrganizationServiceFactory serviceFactory =
                    (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

                try
                {
                    CrmDataHelper objHelper = new CrmDataHelper();
                    Guid accountId = Guid.Empty;
                    Guid contactid=Guid.Empty;
                    Guid addressId = Guid.Empty;
                    int addressNumber = 0;
                    int objectTypeCode=0;
                    bool blnupdateaddressnumber = false;    
                    Entity customAddressObject;
                    
                    if(entity.Contains(CustomAddressAttributes.poad_addressnumber) &&  entity[CustomAddressAttributes.poad_addressnumber]!=null)
                    {
                        addressNumber = (int)entity[CustomAddressAttributes.poad_addressnumber];
                    }
                    
                    if(entity.Contains(CustomAddressAttributes.poad_customerid) && entity[CustomAddressAttributes.poad_customerid] !=null)
                    {

                        EntityReference customer = ((EntityReference)entity[CustomAddressAttributes.poad_customerid]);
                        if(customer.LogicalName=="contact")
                        {
                            objectTypeCode = 2;
                        }
                          else if(customer.LogicalName=="account")
                        {
                            objectTypeCode = 1;
                        }
                        addressId= objHelper.GetAddressId(service, addressNumber, customer.Id);
                        if (addressNumber == 0)
                        {
                            addressNumber = objHelper.GetMaxAddressNumber(service, tracingService, customer.Id, objectTypeCode);
                            blnupdateaddressnumber = true;
                        }
                    }
                    //else if(entity.Contains(CustomAddressAttributes.poad_contactid) && entity[CustomAddressAttributes.poad_contactid] != null)
                    //{
                    //    objectTypeCode = 2;
                    //    contactid = ((EntityReference)entity[CustomAddressAttributes.poad_contactid]).Id;
                    //    addressId = objHelper.GetAddressId(service, addressNumber, contactid);
                    //    if (addressNumber == 0)
                    //    {
                    //        addressNumber = objHelper.GetMaxAddressNumber(service, tracingService, contactid, objectTypeCode);
                    //        blnupdateaddressnumber = true;
                    //    }
                    //}
                     
                        tracingService.Trace("Object Type code" + objectTypeCode);
                    tracingService.Trace("ADdress number::" + addressNumber);
                    if(objectTypeCode!=1 && objectTypeCode!=2)
                    {
                        return;
                    }
                    if(addressNumber ==0)
                    {
                        return;
                    }
                        tracingService.Trace("message" + context.MessageName);
                        
                        
                        if (context.MessageName == "Create" || context.MessageName == "Update")
                        {
                            if (addressId == Guid.Empty)
                            {
                                customAddressObject = CreateAddressObject(entity, objectTypeCode,addressNumber);
                                service.Create(customAddressObject);
                            }
                            else
                            {
                                customAddressObject = CreateAddressObject(entity, objectTypeCode,addressNumber);
                                customAddressObject.Id = addressId;
                                service.Update(customAddressObject);
                            }
                        }
                        if(context.MessageName=="Create" && blnupdateaddressnumber ==true)
                         {
                        entity[CustomAddressAttributes.poad_addressnumber] = addressNumber;
                        service.Update(entity);
                         }
                    

                }
                catch (Exception ex)
                {
                    tracingService.Trace("HandleCustom OOB Address: {0}", ex.ToString());
                    throw;
                }
                //catch (FaultException<OrganizationServiceFault> ex)
                //{
                //    throw new InvalidPluginExecutionException("An error occurred in HandleCustomAddress.", ex);
                //}

              

            }



        }

        private Entity CreateAddressObject(Entity entity, int objectTypeCode,int addressNumber)
        {
            Entity AddressEntity = new Entity("customeraddress");

            if (objectTypeCode == 1)
            {
                AddressEntity[AddressAttributes.parentid] = new EntityReference("account", ((EntityReference)entity[CustomAddressAttributes.poad_customerid]).Id);
                AddressEntity[AddressAttributes.objecttypecode] = "account";
            }
            else
            {
                AddressEntity[AddressAttributes.parentid] = new EntityReference("contact", ((EntityReference)entity[CustomAddressAttributes.poad_customerid]).Id);
                AddressEntity[AddressAttributes.objecttypecode] = "contact";
            }
            AddressEntity[AddressAttributes.addressnumber] = addressNumber;

            if (entity.Contains(CustomAddressAttributes.poad_addresstypecode))
                AddressEntity[AddressAttributes.addresstypecode] = entity[CustomAddressAttributes.poad_addresstypecode] != null ? ((OptionSetValue)entity[CustomAddressAttributes.poad_addresstypecode]) : null;

            if (entity.Contains(CustomAddressAttributes.poad_city))
                AddressEntity[AddressAttributes.city] = entity[CustomAddressAttributes.poad_city]?.ToString();
            //customAddressEntity.poad_composite = entity[AddressAttributes.com] != null ? entity[AddressAttributes.city].ToString() : null;

            if (entity.Contains(CustomAddressAttributes.poad_country))
                AddressEntity[AddressAttributes.country] = entity[CustomAddressAttributes.poad_country]?.ToString();

            if (entity.Contains(CustomAddressAttributes.poad_county))
                AddressEntity[AddressAttributes.county] = entity[CustomAddressAttributes.poad_county]?.ToString();

            if (entity.Contains(CustomAddressAttributes.poad_exchangerate))
                AddressEntity[AddressAttributes.exchangerate] = (decimal?)entity[CustomAddressAttributes.poad_exchangerate];

            if (entity.Contains(CustomAddressAttributes.poad_fax))
                AddressEntity[AddressAttributes.fax] = entity[CustomAddressAttributes.poad_fax]?.ToString();

            if (entity.Contains(CustomAddressAttributes.poad_freighttermscode))
                AddressEntity[AddressAttributes.freighttermscode] = entity[CustomAddressAttributes.poad_freighttermscode] != null ? ((OptionSetValue)entity[CustomAddressAttributes.poad_freighttermscode]) : null;

            if (entity.Contains(CustomAddressAttributes.poad_latitude))
                AddressEntity[AddressAttributes.latitude] = (double?)entity[CustomAddressAttributes.poad_latitude];

            if (entity.Contains(CustomAddressAttributes.poad_line1))
                AddressEntity[AddressAttributes.line1] = entity[CustomAddressAttributes.poad_line1]?.ToString();

            if (entity.Contains(CustomAddressAttributes.poad_line2))
                AddressEntity[AddressAttributes.line2] = entity[CustomAddressAttributes.poad_line2]?.ToString();

            if (entity.Contains(CustomAddressAttributes.poad_line3))
                AddressEntity[AddressAttributes.line3] = entity[CustomAddressAttributes.poad_line3]?.ToString();

            if (entity.Contains(CustomAddressAttributes.poad_longitude))
                AddressEntity[AddressAttributes.longitude] = (double?)entity[CustomAddressAttributes.poad_longitude];
            
            if (entity.Contains(CustomAddressAttributes.poad_name))
                AddressEntity[AddressAttributes.name] = entity[CustomAddressAttributes.poad_name]?.ToString();

            if (entity.Contains(CustomAddressAttributes.poad_postalcode))
                AddressEntity[AddressAttributes.postalcode] = entity[CustomAddressAttributes.poad_postalcode]?.ToString();
            
            if (entity.Contains(CustomAddressAttributes.poad_postofficebox))
                AddressEntity[AddressAttributes.postofficebox] = entity[CustomAddressAttributes.poad_postofficebox]?.ToString();
            
            if (entity.Contains(CustomAddressAttributes.poad_primarycontactname))
                AddressEntity[AddressAttributes.primarycontactname] = entity[CustomAddressAttributes.poad_primarycontactname]?.ToString();
            
            if (entity.Contains(CustomAddressAttributes.poad_shippingmethodcode))
                AddressEntity[AddressAttributes.shippingmethodcode] = entity[CustomAddressAttributes.poad_shippingmethodcode] != null ? ((OptionSetValue)entity[CustomAddressAttributes.poad_shippingmethodcode]) : null;
            
            if (entity.Contains(CustomAddressAttributes.poad_stateorprovince))
                AddressEntity[AddressAttributes.stateorprovince] = entity[CustomAddressAttributes.poad_stateorprovince]?.ToString();
            
             if (entity.Contains(CustomAddressAttributes.poad_telephone1))
                AddressEntity[AddressAttributes.telephone1]  = entity[CustomAddressAttributes.poad_telephone1]?.ToString();

            if (entity.Contains(CustomAddressAttributes.poad_telephone2))
                AddressEntity[AddressAttributes.telephone2] = entity[CustomAddressAttributes.poad_telephone2]?.ToString();
            
             if (entity.Contains(CustomAddressAttributes.poad_telephone3))
                AddressEntity[AddressAttributes.telephone3] = entity[CustomAddressAttributes.poad_telephone3]?.ToString();
            
            if (entity.Contains(CustomAddressAttributes.poad_transactioncurrencyid))
                AddressEntity[AddressAttributes.transactioncurrencyid] = new EntityReference("transactioncurrency", ((EntityReference)entity[CustomAddressAttributes.poad_transactioncurrencyid]).Id);
            
            if (entity.Contains(CustomAddressAttributes.poad_upszone))
                AddressEntity[AddressAttributes.upszone] = entity[CustomAddressAttributes.poad_upszone]?.ToString();
            
            if (entity.Contains(CustomAddressAttributes.poad_utcoffset))
                AddressEntity[AddressAttributes.utcoffset] = (int?)entity[CustomAddressAttributes.poad_utcoffset];

            return AddressEntity;



        }
    }
}
