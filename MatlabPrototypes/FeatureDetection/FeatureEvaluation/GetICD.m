function [ICD] = GetICD( data, lables )
	% Automatically sizes data
	featureSize = size(data,2);
	ICD = zeros (featureSize,3);

	[data2,data1,data0]=SplitData( data, lables );
	for j = 1 : featureSize
		% j is the feature number
		u2 = mean(data2(:,j)); std2 = var(data2(:,j)).^0.5;
		u1 = mean(data1(:,j)); std1 = var(data1(:,j)).^0.5;
		u0 = mean(data0(:,j)); std0 = var(data(:,j)).^0.5;

		ICD(j,1) = (u0 - u1) * (inv(std0+std1)) * transpose(u0 - u1);
		ICD(j,2) = (u0 - u2) * (inv(std0+std2)) * transpose(u0 - u2);
		ICD(j,3) = (u1 - u2) * (inv(std1+std2)) * transpose(u1 - u2);

	end
	
	ICD=ICD';


end

