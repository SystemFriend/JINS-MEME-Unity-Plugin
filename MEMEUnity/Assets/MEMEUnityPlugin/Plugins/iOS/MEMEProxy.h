#import <UIKit/UIKit.h>
#import <MEMELib/MEMELib.h>

@interface MEMEProxy : NSObject<MEMELibDelegate>

@property (strong, nonatomic) MEMERealTimeData *latestRealTimeData;

void MEMEStartSession(char *appClientId, char *appClientSecret);
void MEMEEndSession();
char* MEMEGetSensorValues(char* peripheralUUIDStr);

@end
