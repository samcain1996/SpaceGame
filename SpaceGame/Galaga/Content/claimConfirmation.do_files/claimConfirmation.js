/**
 * 
 */

$(function() {
	$('[data-toggle="tooltip"]').tooltip();
    $('[data-toggle="popover"]').popover(); 
    
    $( "#ccAccordion" ).accordion({
		heightStyle: "content",	
		icons: { "header": "ui-icon-plus", "activeHeader": "ui-icon-minus" },
		collapsible:true,
		active:false
	});
});


function expandAllAndPrint() {
	
	$(".ui-accordion-content").show();
	var claimConfirmationDiv = $(".claimConfirmationDiv").html();
	claimConfirmationDiv.replace("\n", "");
	claimConfirmationDiv.replace("\t", "");
	console.log(claimConfirmationDiv);

	var css = '@page { size: landscape; }',
    head = document.head || document.getElementsByTagName('head')[0],
    style = document.createElement('style');
	style.type = 'text/css';
	style.media = 'print';
	if (style.styleSheet){
	  style.styleSheet.cssText = css;
	} else {
	  style.appendChild(document.createTextNode(css));
	}
	head.appendChild(style);
	
	window.print();
	$(".ui-accordion-content").hide();
	return false;
}

// CR2229 - Print Preview Internet Claim, initial snapshot.
var saveConfirmationHTML = function(claimId, confirmationHTML) {
	var isSuccess = false;
	confirmationHTML = confirmationHTML.replace(/\s/g, "&nbsp;");
	$.ajax({
		type : "POST",
		url : contextPath + "/saveConfirmation.do",
		data : {
			claimHTML:confirmationHTML.toString(),
			claimId : claimId.toString()
		},
		success : function(data) {
			if (data) {
				if (data.indexOf("error:") > -1)
				{
					isSuccess = false;
				}else{
					isSuccess = true;
				}
			}
			
			if (!isSuccess) {
				console.log("CLAIM CONFIRMATION SEND TO SERVER............FAIL");
				return false;
			} else {
				console.log("CLAIM CONFIRMATION SEND TO SERVER............PASS");
				return true;
			}
		}
	});
};