// Begin

// NOTE: If you use a ' add a slash before it like this \'


var menusidetop    = "right"			// MENU SIDE | left | right | center
var showphone      = "yes"			// SHOW PHONE NUMBER
var phonetext      = "Phone: 847.555.5555"	// TEXT FOR PHONE
var phonelink      = "contact.htm"		// LINK FOR PHONE TEXT


document.write('<table class="fullwidth"><tr><td class="top-menu-area">');
   if (showphone == "yes") {
       //document.write('<a href="' + phonelink + '" class="top-phone-link">' + phonetext + '</a>');
       //document.write('<span class="headertitle">River Valley Community Church</span>');
       //document.write('<span class="headertext">');
       //document.write('<br /><a href="Contact.aspx">888 S Edgelawn | Aurora, IL 60506 | 630-844-9115</a>');
       //document.write('<br />questions@rivervalleycommunity.org');
       //document.write('<br />Sunday Worship at 10:00AM');
       //document.write('</span>');
       
document.write('</td><td class="top-menu-area">');
}
document.write('<table class="top-menu-inner '+menusidetop+'-div"><tr><td></td><td>');


// START LINKS

document.write('<span class="headertitle">River Valley Community Church</span>');
document.write('<span class="headertext">');
document.write('<br /><a href="Contact.aspx">888 S Edgelawn | Aurora, IL 60506 | 630-844-9115</a>');
document.write('<br />questions@rivervalleycommunity.org');
document.write('<br />Sunday Worship at 10:00AM');
document.write('</span>');



//document.write('<a href="default.aspx" class="menu-top">Home</a>');
//document.write('</td><td>|</td><td>');


//document.write('<a href="site_map.htm" class="menu-top">Site Map</a>');
//document.write('</td><td>|</td><td>');


//document.write('<a href="PDFgallery.htm" class="menu-top">PDFs</a>');
//document.write('</td><td>|</td><td>');


//document.write('<a href="resources.htm" class="menu-top">Resources</a>');
//document.write('</td><td>|</td><td>');


//document.write('<a href="quotes.htm" class="menu-top">Quotes</a>');
//document.write('</td><td>|</td><td>');


//document.write('<a href="careers.htm" class="menu-top">Careers</a>');
//document.write('</td><td>|</td><td>');


// NOTE: COPY AND PASTE THE NEXT LINE TO MAKE A NEW LINK


//document.write('<a href="contact.htm" class="menu-top">Contact Us</a>');
//document.write('</td><td>|</td><td>');


// END MENU LINKS

document.write('&nbsp;<br></td></tr></table>');
document.write('</td></tr></table>');
