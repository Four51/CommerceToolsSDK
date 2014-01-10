﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34003
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.34003.
// 
#pragma warning disable 1591

namespace Four51.APISDK.Four51Address {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.33440")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="AddressSoap", Namespace="http://four51.com/services/address")]
    public partial class Address : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback SaveOperationCompleted;
        
        private System.Threading.SendOrPostCallback DeleteOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public Address() {
            this.Url = global::Four51.APISDK.Properties.Settings.Default.Core_Four51Address_Address;
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
        public event SaveCompletedEventHandler SaveCompleted;
        
        /// <remarks/>
        public event DeleteCompletedEventHandler DeleteCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://four51.com/services/address/Save", RequestNamespace="http://four51.com/services/address", ResponseNamespace="http://four51.com/services/address", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlArrayItemAttribute(IsNullable=false)]
        public ErrorReport[] Save([System.Xml.Serialization.XmlArrayItemAttribute(IsNullable=false)] AddressProperties[] Addresses, string SharedSecret) {
            object[] results = this.Invoke("Save", new object[] {
                        Addresses,
                        SharedSecret});
            return ((ErrorReport[])(results[0]));
        }
        
        /// <remarks/>
        public void SaveAsync(AddressProperties[] Addresses, string SharedSecret) {
            this.SaveAsync(Addresses, SharedSecret, null);
        }
        
        /// <remarks/>
        public void SaveAsync(AddressProperties[] Addresses, string SharedSecret, object userState) {
            if ((this.SaveOperationCompleted == null)) {
                this.SaveOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSaveOperationCompleted);
            }
            this.InvokeAsync("Save", new object[] {
                        Addresses,
                        SharedSecret}, this.SaveOperationCompleted, userState);
        }
        
        private void OnSaveOperationCompleted(object arg) {
            if ((this.SaveCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SaveCompleted(this, new SaveCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://four51.com/services/address/Delete", RequestNamespace="http://four51.com/services/address", ResponseNamespace="http://four51.com/services/address", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlArrayItemAttribute(IsNullable=false)]
        public ErrorReport[] Delete([System.Xml.Serialization.XmlArrayItemAttribute(IsNullable=false)] AddressDelete[] Addresses, string SharedSecret) {
            object[] results = this.Invoke("Delete", new object[] {
                        Addresses,
                        SharedSecret});
            return ((ErrorReport[])(results[0]));
        }
        
        /// <remarks/>
        public void DeleteAsync(AddressDelete[] Addresses, string SharedSecret) {
            this.DeleteAsync(Addresses, SharedSecret, null);
        }
        
        /// <remarks/>
        public void DeleteAsync(AddressDelete[] Addresses, string SharedSecret, object userState) {
            if ((this.DeleteOperationCompleted == null)) {
                this.DeleteOperationCompleted = new System.Threading.SendOrPostCallback(this.OnDeleteOperationCompleted);
            }
            this.InvokeAsync("Delete", new object[] {
                        Addresses,
                        SharedSecret}, this.DeleteOperationCompleted, userState);
        }
        
        private void OnDeleteOperationCompleted(object arg) {
            if ((this.DeleteCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.DeleteCompleted(this, new DeleteCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://four51.com/services/address")]
    public partial class AddressProperties {
        
        private string interopIDField;
        
        private string companyInteropIDField;
        
        private string addressNameField;
        
        private string firstNameField;
        
        private string lastNameField;
        
        private string companyNameField;
        
        private string addressLine1Field;
        
        private string addressLine2Field;
        
        private string cityField;
        
        private string stateField;
        
        private string zipField;
        
        private string countryField;
        
        private string phoneField;
        
        /// <remarks/>
        public string InteropID {
            get {
                return this.interopIDField;
            }
            set {
                this.interopIDField = value;
            }
        }
        
        /// <remarks/>
        public string CompanyInteropID {
            get {
                return this.companyInteropIDField;
            }
            set {
                this.companyInteropIDField = value;
            }
        }
        
        /// <remarks/>
        public string AddressName {
            get {
                return this.addressNameField;
            }
            set {
                this.addressNameField = value;
            }
        }
        
        /// <remarks/>
        public string FirstName {
            get {
                return this.firstNameField;
            }
            set {
                this.firstNameField = value;
            }
        }
        
        /// <remarks/>
        public string LastName {
            get {
                return this.lastNameField;
            }
            set {
                this.lastNameField = value;
            }
        }
        
        /// <remarks/>
        public string CompanyName {
            get {
                return this.companyNameField;
            }
            set {
                this.companyNameField = value;
            }
        }
        
        /// <remarks/>
        public string AddressLine1 {
            get {
                return this.addressLine1Field;
            }
            set {
                this.addressLine1Field = value;
            }
        }
        
        /// <remarks/>
        public string AddressLine2 {
            get {
                return this.addressLine2Field;
            }
            set {
                this.addressLine2Field = value;
            }
        }
        
        /// <remarks/>
        public string City {
            get {
                return this.cityField;
            }
            set {
                this.cityField = value;
            }
        }
        
        /// <remarks/>
        public string State {
            get {
                return this.stateField;
            }
            set {
                this.stateField = value;
            }
        }
        
        /// <remarks/>
        public string Zip {
            get {
                return this.zipField;
            }
            set {
                this.zipField = value;
            }
        }
        
        /// <remarks/>
        public string Country {
            get {
                return this.countryField;
            }
            set {
                this.countryField = value;
            }
        }
        
        /// <remarks/>
        public string Phone {
            get {
                return this.phoneField;
            }
            set {
                this.phoneField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://four51.com/services/address")]
    public partial class AddressDelete {
        
        private string addressInteropIDField;
        
        private string companyInteropIDField;
        
        private bool overrideOrderReferenceConflictField;
        
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
        public string CompanyInteropID {
            get {
                return this.companyInteropIDField;
            }
            set {
                this.companyInteropIDField = value;
            }
        }
        
        /// <remarks/>
        public bool OverrideOrderReferenceConflict {
            get {
                return this.overrideOrderReferenceConflictField;
            }
            set {
                this.overrideOrderReferenceConflictField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://four51.com/services/address")]
    public partial class ErrorReport {
        
        private string interopIDField;
        
        private string errorMessageField;
        
        /// <remarks/>
        public string InteropID {
            get {
                return this.interopIDField;
            }
            set {
                this.interopIDField = value;
            }
        }
        
        /// <remarks/>
        public string ErrorMessage {
            get {
                return this.errorMessageField;
            }
            set {
                this.errorMessageField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.33440")]
    public delegate void SaveCompletedEventHandler(object sender, SaveCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.33440")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SaveCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SaveCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public ErrorReport[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((ErrorReport[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.33440")]
    public delegate void DeleteCompletedEventHandler(object sender, DeleteCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.33440")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class DeleteCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal DeleteCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public ErrorReport[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((ErrorReport[])(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591