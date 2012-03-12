function [ rms_icd] = GetICD( data, lables )

	% Automatically sizes data
	datasize = size(data);

	ICD = zeros (datasize(2),3,3);

	for i = 1 : 3
		% i is critical component number
		data2_ind= lables(:,i) == 2;
		data1_ind= lables(:,i) == 1;
		data0_ind= lables(:,i) == 0;
		data2=data(data2_ind, :);
		data1=data(data1_ind, :);
		data0=data(data0_ind, :);

		for j = 1 : datasize(2)
			% j is the feature number
			u2 = mean(data2(:,j));
			covar2 = cov(data2(:,j));
			u1 = mean(data1(:,j));
			covar1 = cov(data1(:,j));
			u0 = mean(data0(:,j));
			covar0 = cov(data(:,j));

			Icovar1 = (length(data0(:,j)) * covar0 / datasize(1)) +  (length(data1(:,j)) * covar1 / datasize(1));
			Icovar2 = (length(data0(:,j)) * covar0 / datasize(1)) +  (length(data2(:,j)) * covar2 / datasize(1));
			Icovar3 = (length(data1(:,j)) * covar1 / datasize(1)) +  (length(data2(:,j)) * covar2 / datasize(1));

			ICD(j,1,i) = (u0 - u1) * (inv(Icovar1)) * transpose(u0 - u1);
			ICD(j,2,i) = (u0 - u2) * (inv(Icovar2)) * transpose(u0 - u2);
			ICD(j,3,i) = (u1 - u2) * (inv(Icovar3)) * transpose(u1 - u2);

		end

	end
	rms_icd=zeros(size(ICD,1),3);
	rms_icd(:,1)=sum(ICD(:,:,1).^2,2).^0.5;
	rms_icd(:,2)=sum(ICD(:,:,2).^2,2).^0.5;
	rms_icd(:,3)=sum(ICD(:,:,3).^2,2).^0.5;
	
	rms_icd=sum(rms_icd.^2,2).^0.5;
	


end

