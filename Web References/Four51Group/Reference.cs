﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.17929.
// 
#pragma warning disable 1591

namespace Four51.APISDK.Four51Group {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="GroupSoap", Namespace="http://four51.com/services/group")]
    public partial class Group : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback DeleteOperationCompleted;
        
        private System.Threading.SendOrPostCallback SaveOperationCompleted;
        
        private System.Threading.SendOrPostCallback AssignAddressOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public Group() {
            this.Url = global::Four51.APISDK.Properties.Settings.Default.Core_Four51WebGroup_Group;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event DeleteCompletedEventHandler DeleteCompleted;
        
        /// <remarks/>
        public event SaveCompletedEventHandler SaveCompleted;
        
        /// <remarks/>
        public event AssignAddressCompletedEventHandler AssignAddressCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://four51.com/services/group/Delete", RequestNamespace="http://four51.com/services/group", ResponseNamespace="http://four51.com/services/group", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void Delete(string BuyerInteropID, string GroupInteropID, string SharedSecret) {
            this.Invoke("Delete", new object[] {
                        BuyerInteropID,
                        GroupInteropID,
                        SharedSecret});
        }
        
        /// <remarks/>
        public void DeleteAsync(string BuyerInteropID, string GroupInteropID, string SharedSecret) {
            this.DeleteAsync(BuyerInteropID, GroupInteropID, SharedSecret, null);
        }
        
        /// <remarks/>
        public void DeleteAsync(string BuyerInteropID, string GroupInteropID, string SharedSecret, object userState) {
            if ((this.DeleteOperationCompleted == null)) {
                this.DeleteOperationCompleted = new System.Threading.SendOrPostCallback(this.OnDeleteOperationCompleted);
            }
            this.InvokeAsync("Delete", new object[] {
                        BuyerInteropID,
                        GroupInteropID,
                        SharedSecret}, this.DeleteOperationCompleted, userState);
        }
        
        private void OnDeleteOperationCompleted(object arg) {
            if ((this.DeleteCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.DeleteCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://four51.com/services/group/Save", RequestNamespace="http://four51.com/services/group", ResponseNamespace="http://four51.com/services/group", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void Save(string BuyerInteropID, string GroupInteropID, string Name, string Description, bool IsReportingGroup, string SharedSecret) {
            this.Invoke("Save", new object[] {
                        BuyerInteropID,
                        GroupInteropID,
                        Name,
                        Description,
                        IsReportingGroup,
                        SharedSecret});
        }
        
        /// <remarks/>
        public void SaveAsync(string BuyerInteropID, string GroupInteropID, string Name, string Description, bool IsReportingGroup, string SharedSecret) {
            this.SaveAsync(BuyerInteropID, GroupInteropID, Name, Description, IsReportingGroup, SharedSecret, null);
        }
        
        /// <remarks/>
        public void SaveAsync(string BuyerInteropID, string GroupInteropID, string Name, string Description, bool IsReportingGroup, string SharedSecret, object userState) {
            if ((this.SaveOperationCompleted == null)) {
                this.SaveOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSaveOperationCompleted);
            }
            this.InvokeAsync("Save", new object[] {
                        BuyerInteropID,
                        GroupInteropID,
                        Name,
                        Description,
                        IsReportingGroup,
                        SharedSecret}, this.SaveOperationCompleted, userState);
        }
        
        private void OnSaveOperationCompleted(object arg) {
            if ((this.SaveCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SaveCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://four51.com/services/group/AssignAddress", RequestNamespace="http://four51.com/services/group", ResponseNamespace="http://four51.com/services/group", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void AssignAddress(string BuyerCompanyInteropID, string GroupInteropID, AddressAssignment AddressAssignment, string SharedSecret) {
            this.Invoke("AssignAddress", new object[] {
                        BuyerCompanyInteropID,
                        GroupInteropID,
                        AddressAssignment,
                        SharedSecret});
        }
        
        /// <remarks/>
        public void AssignAddressAsync(string BuyerCompanyInteropID, string GroupInteropID, AddressAssignment AddressAssignment, string SharedSecret) {
            this.AssignAddressAsync(BuyerCompanyInteropID, GroupInteropID, AddressAssignment, SharedSecret, null);
        }
        
        /// <remarks/>
        public void AssignAddressAsync(string BuyerCompanyInteropID, string GroupInteropID, AddressAssignment AddressAssignment, string SharedSecret, object userState) {
            if ((this.AssignAddressOperationCompleted == null)) {
                this.AssignAddressOperationCompleted = new System.Threading.SendOrPostCallback(this.OnAssignAddressOperationCompleted);
            }
            this.InvokeAsync("AssignAddress", new object[] {
                        BuyerCompanyInteropID,
                        GroupInteropID,
                        AddressAssignment,
                        SharedSecret}, this.AssignAddressOperationCompleted, userState);
        }
        
        private void OnAssignAddressOperationCompleted(object arg) {
            if ((this.AssignAddressCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.AssignAddressCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://four51.com/services/group")]
    public partial class AddressAssignment {
        
        private string addressInteropIDField;
        
        private bool useForShippingField;
        
        private bool useForBillingField;
        
        /// <remarks/>
        public string AddressInteropID {
            get {
                return this.addressInteropIDField;
            }
            set {
                this.addressInteropIDField = value;
            }
        }
        
        /// <remarks/>
        public bool UseForShipping {
            get {
                return this.useForShippingField;
            }
            set {
                this.useForShippingField = value;
            }
        }
        
        /// <remarks/>
        public bool UseForBilling {
            get {
                return this.useForBillingField;
            }
            set {
                this.useForBillingField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void DeleteCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void SaveCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void AssignAddressCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
}

#pragma warning restore 1591