using System;

namespace Business.Common.Errors {
	public static class Exceptions {
		public static Exception UserDoesNotHaveModerationAccess = new Exception("The current user does not have Moderation Access.");
		public static Exception InstanceNotFound = new Exception("An instance of the current object is required, but cannot be found in the database.");
		public static Exception ModuleInstances_CannotBeCreated_or_Edited_WithoutAnId = new Exception("Module Instances cannot be created or edited without an id");
		
		public static Exception InstanceIdIsRequired(string missingPropertyName) {
			return new Exception(string.Concat("An instance id is required: ", missingPropertyName));
		}

		public static Exception DatabaseAlreadyExists(string databaseName) {
			return new Exception(string.Concat("This database cannot be created because an instance with this name already exists: ", databaseName));
		}

		public static Exception CouldNotParse = new Exception("The current item could not be parsed.");

		public static Exception EnumNotParsed(Type type, string value) {
			return new Exception(string.Concat("Enum of type ( ", type.Name, ") not parsed. Input value: ", value));
		}

		public static Exception CurrentUserDoesNotExist = new Exception("The system expects a current logged in user, however that user cannot be found at this time.");

		public static Exception InstanceCannotBeCreatedBecauseItIsNotNew = new Exception("Instance cannot be created because it is not new.");
		public static Exception InstanceCannotBeUpdatedBecauseItIsNew = new Exception("Instance cannot be updated because it is new.");

		public static Exception NotificationProfileNotValid = new Exception("From address and SMTP Server may not be null: Invalid Notification Profile.");

		public static Exception ValueNotProvided = new Exception("An importanat value was not provided.");

		public static Exception CurrentUserCannotLogin = new Exception("Windows Service user cannot login.");
		public static Exception EnumTypeNotMatched = new Exception("Enum Type Not Matched. Examine code for new types that haven't been added to this switch statement.");

		public static Exception ValueCannotBeZero(string parameterName) {
			var msg = string.Format("Value Cannot Be Zero for {0}.", parameterName);
			return new Exception(msg);
		}
	}
}