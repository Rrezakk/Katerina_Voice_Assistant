import sys
import requests
from urllib.parse import urlencode
link = sys.argv[1]
filename = sys.argv[2]
base_url = 'https://cloud-api.yandex.net/v1/disk/public/resources/download?'
public_key = link
final_url = base_url + urlencode(dict(public_key=public_key))
response = requests.get(final_url)
download_url = response.json()['href']
download_response = requests.get(download_url)
with open(filename , 'wb') as f:
	f.write(download_response.content)
print('ok')