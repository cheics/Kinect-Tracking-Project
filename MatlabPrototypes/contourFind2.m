function contourFind2(dataIn, critComp, useFeatures)

mapPower=1.2;
nContour=30;

critCompScore=dataIn.classes(:,critComp);
i2=find(critCompScore==2); i1=find(critCompScore==1); i0=find(critCompScore==0);

figure;
plot3(dataIn.features(i2,useFeatures(1)), dataIn.features(i2,useFeatures(2)), dataIn.features(i2,useFeatures(3)), 'g.',...
	dataIn.features(i1,useFeatures(1)), dataIn.features(i1,useFeatures(2)), dataIn.features(i1,useFeatures(3)),'b.',...
	dataIn.features(i0,useFeatures(1)), dataIn.features(i0,useFeatures(2)), dataIn.features(i0,useFeatures(3)),'r.')


%% Graph KDEs of each cluster

[bandwidth2,density2,X2,Y2]=kde2d( ...
	[dataIn.features(i2,useFeatures(1)),...
	dataIn.features(i2,useFeatures(2))] ...
);
[bandwidth1,density1,X1,Y1]=kde2d( ...
	[dataIn.features(i1,useFeatures(1)),...
	dataIn.features(i1,useFeatures(2))] ...
);
[bandwidth0,density0,X0,Y0]=kde2d( ...
	[dataIn.features(i0,useFeatures(1)),...
	dataIn.features(i0,useFeatures(2))] ...
);

xx=[min(min([X2,X1,X0])), max(max([X2,X1,X0]))];
yy=[min(min([Y2,Y1,Y0])), max(max([Y2,Y1,Y0]))];

figure;
hold on;
contourf(X2,Y2,density2,nContour);
colormap (c_colourMap('green').^mapPower);
xlim(xx); ylim(yy);
shading flat;
plot(dataIn.features(i2,useFeatures(1)), dataIn.features(i2,useFeatures(2)), 'g.',...
	dataIn.features(i1,useFeatures(1)), dataIn.features(i1,useFeatures(2)), 'b.',...
	dataIn.features(i0,useFeatures(1)), dataIn.features(i0,useFeatures(2)), 'r.');
hold off;

figure;
hold on;
contourf(X1,Y1,density1,nContour);
colormap (c_colourMap('blue').^mapPower);
xlim(xx); ylim(yy);
shading flat;
plot(dataIn.features(i2,useFeatures(1)), dataIn.features(i2,useFeatures(2)), 'g.',...
	dataIn.features(i1,useFeatures(1)), dataIn.features(i1,useFeatures(2)), 'b.',...
	dataIn.features(i0,useFeatures(1)), dataIn.features(i0,useFeatures(2)), 'r.');
hold off;

figure;
hold on;
contourf(X0,Y0,density0,nContour);
colormap (c_colourMap('red').^mapPower);
xlim(xx); ylim(yy);
shading flat;
plot(dataIn.features(i2,useFeatures(1)), dataIn.features(i2,useFeatures(2)), 'g.',...
	dataIn.features(i1,useFeatures(1)), dataIn.features(i1,useFeatures(2)), 'b.',...
	dataIn.features(i0,useFeatures(1)), dataIn.features(i0,useFeatures(2)), 'r.');
hold off;
end
