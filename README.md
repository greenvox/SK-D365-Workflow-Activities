# SK-D365-Workflow-Activities
Custom Workflow Activities for D365 (for v8.2)

### Associate OptionSet Value
Associate option sets to a field by their value. A gap fix for limited functionality in workflows.

### Change Process Stage
Change process stage moving forward, backwards, or by crossing multiple stages through imitation.

### Email User On Post Mention
Tagging a user in the timeline doesn't notify the user. This workflow activity allows you to send them an email when they are tagged in the subject or body of a post.

### File Upload
Allows you to upload a file to the notes (annotation) entity using the binary data of the file.

### Get Contact By Attribute
Returns the first contact match based on an attribute value. (e.g Could be used for returning a user based on email address)

### Get Account By Attribute
Returns the first account match based on an attribute value.

### Get Account By FetchXML
Returns the first account match based on fetchXml filter statements. (e.g Could be used for multiple filters)

### Get Lead By Attribute
Returns the first lead match based on an attribute value.

### Get Opportunity By Attribute
Returns the first opportunity match based on an attribute value.

### Get Opportunity Product By Attribute
Returns the first opportunity product match based on an attribute value.

### Get Product By Attribute
Returns the first product match based on an attribute value.

### Get String By Value
Returns a string value from any entity record matched based on any value.

### Get User By Attribute
Returns the first user match based on an attribute value. (e.g Could be used for returning a user based on email address)

### HTTP Post JSON (From Roman Kopaev - https://github.com/ZooY)
Makes a JSON call to any endpoint. Can be used to call Flows using the HTTP connector.

### HTTP Put JSON (From Roman Kopaev - https://github.com/ZooY)
Makes a JSON call to any endpoint. Can be used to call Flows using the HTTP connector.

### HTTP JSON with Header
Makes a GET, POST, PUT, or PATCH call using a JSON Header and a JSON body. Can be used to call any web service that uses JSON content-type.

### Get JSON Value By Path
Get any part of a JSON response though a path. Traverses through nodes, arrays, and allows for the return of multiple data types as well as comma delimited multiple values. To get the path of any JSON body, use https://jsonpathfinder.com/. (Remove the "x" prefix).

### SharePoint Upload File
Upload files directly to any SharePoint document library or list using user and password.

### DocuSign Rest API call
Make a call to the DocuSign API directly using the X-DocuSign-Authentication header. For more details on how to make these calls, visit: https://developers.docusign.com/docs/esign-rest-api/reference/
