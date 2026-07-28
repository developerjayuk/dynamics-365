function ContactOnLoad() {
    alert("This is the Onload event of Form");
}

function ContactSave() {
    alert("This is the Save event of Form");
}

function EmailOnChange() {
    alert("This is the OnChange event of the email attribute");
}

function DisplayName(executionContext) {
    var firstName = formContext.getAttribute("firstname").getValue();
    alert("Name of the contact is " + firstName);
}