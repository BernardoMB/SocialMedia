using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SocialMedia.Core.DTOs
{
    /**
     * Dto is a plain object for passing and reciveing information. 
     * There should be no logic in this file.
     */
    public class PostDto
    {
        // Decorators are very usefull but dto classes should be simpler classes with logic or decorators.

        public int PostId { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        //[Required] // Don't like to use decorators for validations. Instead register custom validatos in Startup.cs.
        // Use validation decorators when we want the [ApiController] decorator of the controller class to automatically validate the modelstate.
        public string Description { get; set; }
        public string Image { get; set; }
    }
}
