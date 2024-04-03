from google.auth.transport.requests import Request
from google.oauth2.service_account import Credentials
from googleapiclient.discovery import build
from googleapiclient.http import MediaFileUpload
import os

# Path to your JSON key file
KEY_FILE_PATH = 'crazycat-417913-588d03717d4b.json'

# Scopes required for the Google Play Developer API
SCOPES = ['https://www.googleapis.com/auth/androidpublisher']

# Package name of your app
PACKAGE_NAME = 'com.gamebotstudio.shapeshifter'

# Path to your AAB file
AAB_FILE_PATH = 'shapeshifter.aab'

def authenticate():
    credentials = Credentials.from_service_account_file(
        KEY_FILE_PATH,
        scopes=SCOPES
    )
    return credentials

def upload_bundle():
    # Authenticate
    creds = authenticate()

    # Build the API service
    service = build('androidpublisher', 'v3', credentials=creds)

    # Create the edit
    edit_request = service.edits().insert(
        body={},
        packageName=PACKAGE_NAME
    )
    edit_response = edit_request.execute()
    edit_id = edit_response['id']

    # Upload the AAB
    media = MediaFileUpload(AAB_FILE_PATH, mimetype='application/octet-stream', resumable=True)
    aab_bundle_response = service.edits().bundles().upload(
        editId=edit_id,
        packageName=PACKAGE_NAME,
        media_body=media
    ).execute()
    aab_version_code = aab_bundle_response['versionCode']
    release_info = {
        'releases': [{
            'versionCodes': [aab_version_code],
            'status': 'completed',
        }]
    }

    # Assign the release to the internal testing track
    track_response = service.edits().tracks().update(
        editId=edit_id,
        track='internal',  # Specify the track as 'internal'
        packageName=PACKAGE_NAME,
        body=release_info
    ).execute()
    
    

    # Commit the edit
    commit_request = service.edits().commit(
        editId=edit_id,
        packageName=PACKAGE_NAME,
        changesNotSentForReview=True
    ).execute()

    print("AAB uploaded successfully with version code:", aab_version_code)

if __name__ == "__main__":
    upload_bundle()
