using System;
using System.Data;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections;

namespace HappyDaySystem.App.CoreMod.Utils
{
    /**
     * Class that contains data replied from server
     * @author Vic
     * @package ServerResponse 
     */
    public class ServerResponse
    {
        /**
         * Operation is success or not
         * @access public
         * @var Success
         */	
	    public bool Success = true;

        /**
         * Array contains all error info
         * @access public
         * @var ErrorInfo
         */	
	    public NameValueCollection ErrorInfo = null;
	
        /**
         * Array contains all replied data
         * @access public
         * @var Data
         */
        public NameValueCollection Data = null;


        //public DataSet ds = null;

        /**
         * Add an error info into ErrorInfo array & set Success to false
         * @param errField : Field name
         * @param errCode : Error code 
         * @return none 
         */
	    public void addErrorInfo(string errField, string errCode)
	    {
		    if(ErrorInfo == null)
			    ErrorInfo = new NameValueCollection();
		
		    Success = false;
		
		    ErrorInfo.Add(errField,errCode);
	    }

        /** 
         * Add an data to m_Data
         * @param data : data
         * @return none 
         */
        public void setResponseData(NameValueCollection data)
	    {
            Data = data;
	    }

       
        ///** 
        // * Add an data to m_Data
        // * @param data : data
        // * @return none 
        // */
        //public void setResponseDataset(DataSet data)
        //{
        //    ds = data;
        //}


    }
}