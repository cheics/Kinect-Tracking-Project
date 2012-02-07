function calibrateCamera(obj,methodNumber)
	if methodNumber == 1
		obj.groundPlane.dir=obj.calibData.camOrientation.gpSkel(1:3);
		obj.groundPlane.loc=obj.groundPlane.dir*-1*obj.calibData.camOrientation.gpSkel(4);
	else
		% Hard coded from measurement session...
		% figure out the trig later...
		obj.groundPlane.dir=[0,1,0];
		obj.groundPlane.loc=[0.0664, -0.90, 2.7400];
	end

end