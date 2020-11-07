﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Microsoft.Xrm.Sdk;
using GKCustomAddress.Helper;
namespace GKCustomAddress
{
    public class HandleCustomAddress :IPlugin
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
            if (context.Depth > 1)
            {
                return;
            }
            //Entity entity = context.PostEntityImages["PostImage"];
            if (context.PostEntityImages.Contains("PostImage") &&
                context.PostEntityImages["PostImage"] is Entity)
            {
                
                Entity entity = (Entity)context.PostEntityImages["PostImage"];
                              
                IOrganizationServiceFactory serviceFactory =
                    (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

                try {
                    CrmDataHelper objHelper = new CrmDataHelper();
                    Guid customerAddressId = Guid.Empty;
                    Guid parentId;
                    int addressNumber;
                    int objectTypeCode;
                    Entity customAddressObject;
                    EntityCollection addresscollection = objHelper.GetCustomerAddress(service, entity.Id);
                    foreach (Entity e in addresscollection.Entities)
                    {
                        
                        if(entity.LogicalName =="contact")
                        {
                            objectTypeCode = 2;
                        }
                        else
                        {
                            objectTypeCode = 1;
                        }
                        
                        addressNumber = (int)e[AddressAttributes.addressnumber];
                        tracingService.Trace("Object Type code" + objectTypeCode);
                        tracingService.Trace("message" + context.MessageName);
                        parentId = ((EntityReference)e[AddressAttributes.parentid]).Id;
                        customerAddressId = objHelper.GetParentIdCustomerAddress(service, addressNumber, entity.Id, objectTypeCode);

                        if (context.MessageName == "Create" || context.MessageName == "Update")
                        {
                            if (customerAddressId == Guid.Empty)
                            {
                                customAddressObject = CreateCustomerAddressObject(e,objectTypeCode);
                                service.Create(customAddressObject);
                            }
                            else
                            {
                                customAddressObject = CreateCustomerAddressObject(e,objectTypeCode);
                                customAddressObject.Id = customerAddressId;
                                service.Update(customAddressObject);
                            }
                        }
                    }
                  
                }
                catch (Exception ex)
                {
                    tracingService.Trace("HandleCustomAddress: {0}", ex.ToString());
                    throw;
                }
                //catch (FaultException<OrganizationServiceFault> ex)
                //{
                //    throw new InvalidPluginExecutionException("An error occurred in HandleCustomAddress.", ex);
                //}

               
                
            }
        }

        private Entity CreateCustomerAddressObject (Entity entity,int objectTypeCode)
        {
            Entity customAddressEntity = new Entity("poad_customaddress");
          
            if (objectTypeCode == 1)
            {
                customAddressEntity[CustomAddressAttributes.poad_customerid] = new EntityReference("account", ((EntityReference)entity[AddressAttributes.parentid]).Id);
            }
            else
            {
                customAddressEntity[CustomAddressAttributes.poad_customerid] = new EntityReference("contact", ((EntityReference)entity[AddressAttributes.parentid]).Id);
            }
            customAddressEntity[CustomAddressAttributes.poad_addressnumber] = Convert.ToInt32(entity[AddressAttributes.addressnumber].ToString());
            if(entity.Contains(AddressAttributes.addresstypecode))
                customAddressEntity[CustomAddressAttributes.poad_addresstypecode] = entity[AddressAttributes.addresstypecode] != null ? ((OptionSetValue)entity[AddressAttributes.addresstypecode]): null;

            if (entity.Contains(AddressAttributes.city))
                customAddressEntity[CustomAddressAttributes.poad_city] = entity[AddressAttributes.city]?.ToString();
            //customAddressEntity.poad_composite = entity[AddressAttributes.com] != null ? entity[AddressAttributes.city].ToString() : null;
            if (entity.Contains(AddressAttributes.country))
                customAddressEntity[CustomAddressAttributes.poad_country] = entity[AddressAttributes.country]?.ToString();
            if (entity.Contains(AddressAttributes.county))
                customAddressEntity[CustomAddressAttributes.poad_county] = entity[AddressAttributes.county]?.ToString();
            if (entity.Contains(AddressAttributes.exchangerate))
                customAddressEntity[CustomAddressAttributes.poad_exchangerate] = (decimal?)entity[AddressAttributes.exchangerate];

            if (entity.Contains(AddressAttributes.fax))
                customAddressEntity[CustomAddressAttributes.poad_fax] = entity[AddressAttributes.fax]?.ToString();
            if (entity.Contains(AddressAttributes.freighttermscode))
                customAddressEntity[CustomAddressAttributes.poad_freighttermscode] = entity[AddressAttributes.freighttermscode] != null ? ((OptionSetValue)entity[AddressAttributes.freighttermscode]) : null;
            if (entity.Contains(AddressAttributes.latitude))
                customAddressEntity[CustomAddressAttributes.poad_latitude] = (float?)entity[AddressAttributes.latitude];
            if (entity.Contains(AddressAttributes.line1))
                customAddressEntity[CustomAddressAttributes.poad_line1] = entity[AddressAttributes.line1]?.ToString();
            if (entity.Contains(AddressAttributes.line2))
                customAddressEntity[CustomAddressAttributes.poad_line2] = entity[AddressAttributes.line2]?.ToString();
            if (entity.Contains(AddressAttributes.line3))
                customAddressEntity[CustomAddressAttributes.poad_line3] = entity[AddressAttributes.line3]?.ToString();
            if (entity.Contains(AddressAttributes.longitude))
                customAddressEntity[CustomAddressAttributes.poad_longitude] = (float?)entity[AddressAttributes.longitude];
            if (entity.Contains(AddressAttributes.name))
                customAddressEntity[CustomAddressAttributes.poad_name] = entity[AddressAttributes.name]?.ToString();

            if (entity.Contains(AddressAttributes.postalcode))
                customAddressEntity[CustomAddressAttributes.poad_postalcode] = entity[AddressAttributes.postalcode]?.ToString();
            if (entity.Contains(AddressAttributes.postofficebox))
                customAddressEntity[CustomAddressAttributes.poad_postofficebox] = entity[AddressAttributes.postofficebox]?.ToString();
            if (entity.Contains(AddressAttributes.primarycontactname))
                customAddressEntity[CustomAddressAttributes.poad_primarycontactname] = entity[AddressAttributes.primarycontactname]?.ToString();
            if (entity.Contains(AddressAttributes.shippingmethodcode))
                customAddressEntity[CustomAddressAttributes.poad_shippingmethodcode] = entity[AddressAttributes.shippingmethodcode] != null ? ((OptionSetValue)entity[AddressAttributes.shippingmethodcode]) : null;
            if (entity.Contains(AddressAttributes.stateorprovince))
                customAddressEntity[CustomAddressAttributes.poad_stateorprovince] = entity[AddressAttributes.stateorprovince]?.ToString();
            if (entity.Contains(AddressAttributes.telephone1))
                customAddressEntity[CustomAddressAttributes.poad_telephone1]  = entity[AddressAttributes.telephone1]?.ToString();
            if (entity.Contains(AddressAttributes.telephone2))
                customAddressEntity[CustomAddressAttributes.poad_telephone2] = entity[AddressAttributes.telephone2]?.ToString();
            if (entity.Contains(AddressAttributes.telephone3))
                customAddressEntity[CustomAddressAttributes.poad_telephone3] = entity[AddressAttributes.telephone3]?.ToString();
            if (entity.Contains(AddressAttributes.transactioncurrencyid))
                customAddressEntity[CustomAddressAttributes.poad_transactioncurrencyid] = new EntityReference("transactioncurrency", ((EntityReference)entity[AddressAttributes.transactioncurrencyid]).Id);
            if (entity.Contains(AddressAttributes.upszone))
                customAddressEntity[CustomAddressAttributes.poad_upszone] = entity[AddressAttributes.upszone]?.ToString();
            if (entity.Contains(AddressAttributes.utcoffset))
                customAddressEntity[CustomAddressAttributes.poad_utcoffset] = (int?)entity[AddressAttributes.utcoffset];

            return customAddressEntity;
            
            
           
        }
    }
}