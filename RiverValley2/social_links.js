// SOCIAL LINKS

// USE lowercase FOR ALL OPTIONS


var googleplus	= "no"		// SHOW GOOGLE PLUS

var googlelink	= "https://plus.google.com/"


var facebook	= "yes"		// SHOW FACEBOOK

var facelink = "https://www.facebook.com/pages/River-Valley-Community-Church/112078325513992"


var twitter	= "yes"		// SHOW TWITTER

var twitlink = "https://twitter.com/RVCCAurora"


var linkedin	= "no"		// SHOW LINKEDIN

var linkedlink	= "http://www.linkedin.com/"


var contactus	= "yes"		// SHOW CONTACT ICON

var contactlink	= "contact.htm"		// CONTACT ICON LINK


var connecttxt	= "yes"		// SHOW TEXT UNDER ICONS
var socalign	= "right"	// ALIGN SOCIAL ICONS (EDIT CSS)
var linktype	= "_blank"	// SOCIAL LINK TYPE USE | _blank | _top |
var linktypec	= "_top"	// CONTACT LINK TYPE USE | _blank | _top |



// COPYRIGHT 2014 © Allwebco Design Corporation
// Unauthorized use or sale of this script is strictly prohibited by law

// YOU DO NOT NEED TO EDIT BELOW THIS LINE


   if ((googleplus == "yes") || (facebook == "yes") || (twitter == "yes") || (linkedin == "yes") || (contactus == "yes")) {

document.write('<table class="social-icon-area printhide '+socalign+'-div"><tr>');

// GOOGLE PLUS

   if (googleplus == "yes") {

document.write('<td class="socialspace"><a href="'+googlelink+'" target="'+linktype+'"><img src="picts/social_googleplus.gif" width="22" height="22" class="blockimage" alt="Add to Google Plus"></a></td>');
}


// FACEBOOK

   if (facebook == "yes") {

document.write('<td class="socialspace"><a href="'+facelink+'" target="'+linktype+'"><img src="picts/social_facebook.gif" width="22" height="22" class="blockimage" alt="Connect on Facebook"></a></td>');
}

// TWITTER

   if (twitter == "yes") {
document.write('<td class="socialspace"><a href="'+twitlink+'" target="'+linktype+'"><img src="picts/social_twitter.gif" width="22" height="22" class="blockimage" alt="Follow us on twitter"></a></td>');
}


// LINKEDIN

   if (linkedin == "yes") {
document.write('<td class="socialspace"><a href="'+linkedlink+'" target="'+linktype+'"><img src="picts/social_linkedin.gif" width="22" height="22" class="blockimage" alt="Connect on LinkedIn"></a></td>');
}


// CONTACT

   if (contactus == "yes") {
document.write('<td class="socialspace"><a href="'+contactlink+'" target="'+linktypec+'"><img src="picts/social_contact.gif" width="22" height="22" class="blockimage" alt="Contact Us"></a></td>');
}


document.write('</tr></table>');
}
   if (connecttxt == "yes") {
document.write('<div class="footer-right-box td-center footertext '+socalign+'-div">Connect With Us</div>');
}
