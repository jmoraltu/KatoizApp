using UnityEngine;
using System.Collections;
using System.Xml;

public class Geolocation : MonoBehaviour {

	public string playerPrefsKey = "Country";

	public void GetGeographicalCoordinates()
	{
		if(Input.location.isEnabledByUser)
			StartCoroutine(GetGeographicalCoordinatesCoroutine());
	}

	private IEnumerator GetGeographicalCoordinatesCoroutine()
	{
		Input.location.Start();
		int maximumWait = 20;
		while(Input.location.status == LocationServiceStatus.Initializing  && maximumWait > 0)
		{
			yield return new WaitForSeconds(1);
			maximumWait--;
		}
		//if(maximumWait < 1 || Input.location.status == LocationServiceStatus.Failed)
		if(maximumWait < 1 && Input.location.status == LocationServiceStatus.Failed)
		{
			Input.location.Stop();
			yield break;
		}
		float latitude = Input.location.lastData.latitude;
		float longitude = Input.location.lastData.longitude;
		//      Asakusa.
		//      float latitude = 35.71477f;
		//      float longitude = 139.79256f;
		Input.location.Stop();
		//LOcation Barcelona
		//https://maps.googleapis.com/maps/api/geocode/xml?latlng=41.3850640,2.1734030
		WWW www = new WWW("https://maps.googleapis.com/maps/api/geocode/xml?latlng=" + latitude + "," + longitude + "&sensor=true");
		yield return www;
		if(www.error != null) yield break;
		XmlDocument reverseGeocodeResult = new XmlDocument();
		reverseGeocodeResult.LoadXml(www.text);
		if(reverseGeocodeResult.GetElementsByTagName("status").Item(0).ChildNodes.Item(0).Value != "OK") yield break;
		string countryCode = string.Empty;
		string countryName = string.Empty;
		bool countryFound = false;
		foreach(XmlNode eachAdressComponent in reverseGeocodeResult.GetElementsByTagName("result").Item(0).ChildNodes)
		{
			if(eachAdressComponent.Name == "address_component")
			{
				foreach(XmlNode eachAddressAttribute in eachAdressComponent.ChildNodes)
				{
					if(eachAddressAttribute.Name == "short_name"){
						countryCode = eachAddressAttribute.FirstChild.Value;
					} 
					if(eachAddressAttribute.Name == "long_name"){
						countryName = eachAddressAttribute.FirstChild.Value;
					}
					if(eachAddressAttribute.Name == "type" ||  eachAddressAttribute.FirstChild.Value == "country"){
						countryFound = true;
					}
				}
				if(countryFound) break;
			}
		}
		if(countryFound && countryCode != null)
			//PlayerPrefs.SetString(playerPrefsKey,countryCode);
			Debug.Log(countryCode);
			Debug.Log(countryName);
	}
}
